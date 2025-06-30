using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;

namespace UISample.Features
{
    public class Shop : ILocalService, IInitializable
    {
        private readonly ShopConfig _config;
        private readonly PlayerData _playerData;
        private ShopController _shopController;
        public bool Initialized { get; private set; }
        
        public Shop(MainMenuConfigs configsContainer)
        {
            _config = configsContainer.ShopConfig;
            _playerData = ServiceLocator.Get<PlayerData>();
        }

        public void Initialize()
        {
            _shopController = ServiceLocator.Get<SceneUI>().GetController<ShopController>();
            _shopController.InitializeSlots();
            Initialized = true;
        }

        public bool TryPurchaseProduct(int index)
        {
            var product = _config.GetProduct(index);
            var cost = product.Cost;
            switch (product.Currency)
            {
                case ECurrency.Acorns:
                    if (_playerData.Acorns.Value < cost)
                        return false;
                    _playerData.Acorns.Value -= cost;
                    return true;
                case ECurrency.Gems:
                    if (_playerData.Gems.Value < cost)
                        return false;
                    _playerData.Gems.Value -= cost;
                    return true;
                case ECurrency.RealMoney:
                    // Handle real money purchase
                    break;
                case ECurrency.Adv:
                    // Handle adv purchase
                    break;
            }
            return false;
        }
        
        public void ConsumeProduct(int index)
        {
            var product = _config.GetProduct(index);
            switch (product.Type)
            {
                case EProducts.Gem:
                    _playerData.Gems.Value += product.Amount;
                    break;
                case EProducts.RandomSkin:
                    // Handle unlock random skin
                    break;
            }
        }
    }
}