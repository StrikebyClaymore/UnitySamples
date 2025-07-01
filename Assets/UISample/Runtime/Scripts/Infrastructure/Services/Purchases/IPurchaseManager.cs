using System;
using Plugins.ServiceLocator;

namespace UISample.Infrastructure
{
    public interface IPurchaseManager : IService, IInitializable
    {
        event Action<string, bool> OnProductPurchased;

        void Purchase(string productId);
        
        void ReceiveProductPurchased(string productId, bool success);
    }
}