using System;
using Plugins.ServiceLocator;

namespace UISample.Infrastructure
{
    public interface IPurchaseManager : IService, IInitializable
    {
        Action<string> OnProductPurchased { get; set; }

        void Purchase();
    }
}