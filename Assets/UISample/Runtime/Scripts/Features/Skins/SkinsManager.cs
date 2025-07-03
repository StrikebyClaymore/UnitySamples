using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;

namespace UISample.Features
{
    public class SkinsManager : ILocalService, IInitializable
    {
        private readonly SkinsConfig _config;
        private readonly PlayerData _playerData;
        private SkinsController _skinsController;
        private ShopController _shopController;
        private Skin[] _skins;
        public IReadOnlyCollection<Skin> Skins => _skins;
        public bool IsInitialized { get; private set; }
        public bool AllSkinsUnlocked => _skins.All(x => x.IsUnlocked);

        public SkinsManager(MainMenuConfigs configsContainer)
        {
            _config = configsContainer.SkinsConfig;
            _playerData = ServiceLocator.Get<PlayerData>();
        }

        public void Initialize()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            _skinsController = sceneUI.GetController<SkinsController>();
            _shopController = sceneUI.GetController<ShopController>();
            LoadOrCreate();
            _skinsController.InitializeSlots(_skins);
            IsInitialized = true;
        }

        public void UnlockSkin(int id)
        {
            _skins[id].IsUnlocked = true;
            SaveData();
            if(AllSkinsUnlocked)
                _shopController.HideSkinSlots();
        }

        private void LoadOrCreate()
        {
            var saveJson = _playerData.Skins.Value;
            if (string.IsNullOrEmpty(saveJson))
            {
                CreateNewSave();
                UnlockSkin(0);
                SaveData();
            }
            else
            {
                _skins = JsonConvert.DeserializeObject<Skin[]>(_playerData.Skins.Value);
            }

            foreach (var skin in _skins)
                skin.Sprite = _config.Skins[skin.Id].Sprite;
        }

        private void SaveData()
        {
            _playerData.Skins.Value = JsonConvert.SerializeObject(_skins);
        }

        private void CreateNewSave()
        {
            _skins = new Skin[_config.Skins.Count];
            for (int i = 0; i < _skins.Length; i++)
            {
                var skin = new Skin(i, false);
                _skins[i] = skin;
            }
        }
    }
}