using System;

namespace UISample.Infrastructure
{
    public class StubPurchaseManager : IPurchaseManager
    {
        public bool IsInitialized { get; private set; }
        private event Action<string, bool> _onProductPurchased;
        public event Action<string, bool> OnProductPurchased
        {
            add => _onProductPurchased += value;
            remove => _onProductPurchased -= value;
        }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void Purchase(string productId)
        {
            ReceiveProductPurchased(productId, true);
        }

        public void ReceiveProductPurchased(string productId, bool success)
        {
            _onProductPurchased?.Invoke(productId, success);
        }
    }
}