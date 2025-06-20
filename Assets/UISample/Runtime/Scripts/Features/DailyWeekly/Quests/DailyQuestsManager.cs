using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;

namespace UISample.Features
{
    public class DailyQuestsManager : ILocalService, IInitializable
    {
        private readonly DailyQuestsConfig _config;
        private readonly QuestsGenerator _generator;
        private readonly PlayerData _playerData;
        private DailyQuestController _dailyQuestController;
        private MainMenuController _mainMenuController;
        public bool Initialized { get; private set; }

        public DailyQuestsManager(MainMenuConfigs configs)
        {
            _config = configs.DailyQuestsConfig;
            _generator = new QuestsGenerator(_config);
            _playerData = ServiceLocator.Get<PlayerData>();
        }
        
        public void Initialize()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            _dailyQuestController = sceneUI.GetController<DailyQuestController>();
            _mainMenuController = sceneUI.GetController<MainMenuController>();
            LoadOrCreateData();
            Initialized = true;
        }
        
        private void LoadOrCreateData()
        {
            
        }

        private void SaveData()
        {
            
        }
    }
}