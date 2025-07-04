﻿using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class HUDController : BaseController
    {
        private readonly HUDView _view;
        
        public HUDController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<HUDView>();
            var gameplayData = ServiceLocator.Get<GameplayData>();
            gameplayData.Acorns.OnValueChanged += UpdateAcorns;
            UpdateAcorns(gameplayData.Acorns.Value);
            _view.PauseButton.onClick.AddListener(PausePressed);
        }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }

        private void UpdateAcorns(int value)
        {
            _view.AcornsText.text = value.ToString();
        }

        private void PausePressed()
        {
            _sceneUI.ShowController<PauseController>();
        }
    }
}