using System;
using Plugins.ServiceLocator;

namespace UISample.Infrastructure
{
    public interface IAdvManager : IService, IInitializable
    {
        event Action<bool> OnRewardAdvShown;

        void ShowRewardAdv();

        void ReceiveRewardAdvShown(bool success);
    }
}