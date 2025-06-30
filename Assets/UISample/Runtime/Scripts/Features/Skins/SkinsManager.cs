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
        private Skin[] _skins;
        public bool IsInitialized { get; private set; }

        public SkinsManager(MainMenuConfigs configsContainer)
        {
            _config = configsContainer.SkinsConfig;
            _playerData = ServiceLocator.Get<PlayerData>();
        }

        public void Initialize()
        {
            _skinsController = ServiceLocator.Get<SceneUI>().GetController<SkinsController>();
            LoadOrCreate();
            _skinsController.InitializeSlots(_skins);
            IsInitialized = true;
        }

        public void UnlockSkin(int id)
        {
            _skins[id].IsUnlocked = true;
            SaveData();
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