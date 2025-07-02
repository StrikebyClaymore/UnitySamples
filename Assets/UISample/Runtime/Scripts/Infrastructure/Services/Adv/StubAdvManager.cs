using System;
using Plugins.ServiceLocator;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class StubAdvManager : IAdvManager
    {
        private readonly AdvPanel _advPanelPrefab;
        private AdvPanel _advPanel;
        public bool IsInitialized { get; private set; }
        private event Action<bool> _onRewardAdvShown;
        public event Action<bool> OnRewardAdvShown
        {
            add => _onRewardAdvShown += value;
            remove => _onRewardAdvShown -= value;
        }

        public StubAdvManager(AdvPanel advPanelPrefab)
        {
            _advPanelPrefab = advPanelPrefab;
        }
        
        public void Initialize()
        {
            IsInitialized = true;
        }

        public void ShowRewardAdv()
        {
            if(_advPanel == null)
                CreateAdvPanel();
            _advPanel.Show();
            _advPanel.OnClose += AdvPanelClosed;
        }

        public void ReceiveRewardAdvShown(bool success)
        {
            _onRewardAdvShown?.Invoke(success);
        }

        private void AdvPanelClosed(bool success)
        {
            _advPanel.OnClose -= AdvPanelClosed;
            ReceiveRewardAdvShown(success);
        }

        private void CreateAdvPanel()
        {
            var parent = ServiceLocator.Get<SceneUI>().RootCanvas;
            _advPanel = GameObject.Instantiate(_advPanelPrefab, parent.transform);
            _advPanel.Hide();
        }
    }
}