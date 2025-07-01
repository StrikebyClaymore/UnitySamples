using System;

namespace UISample.Infrastructure
{
    public class StubAdvManager : IAdvManager
    {
        public bool IsInitialized { get; private set; }
        private event Action<bool> _onRewardAdvShown;
        public event Action<bool> OnRewardAdvShown
        {
            add => _onRewardAdvShown += value;
            remove => _onRewardAdvShown -= value;
        }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void ShowRewardAdv()
        {
            ReceiveRewardAdvShown(true);
        }

        public void ReceiveRewardAdvShown(bool success)
        {
            _onRewardAdvShown?.Invoke(success);
        }
    }
}