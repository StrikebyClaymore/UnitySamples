using System;

namespace UISample.Infrastructure
{
    public class StubPurchaseManager : IPurchaseManager
    {
        public bool IsInitialized { get; private set; }
        public Action<string> OnProductPurchased { get; set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void Purchase()
        {
            
        }
    }
}