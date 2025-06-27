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
    }
}