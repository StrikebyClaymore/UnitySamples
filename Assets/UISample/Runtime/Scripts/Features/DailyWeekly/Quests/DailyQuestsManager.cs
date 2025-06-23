using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;

namespace UISample.Features
{
    public class DailyQuestsManager : IService, IInitializable
    {
        private readonly DailyQuestsConfig _config;
        private readonly QuestsGenerator _generator;
        private readonly PlayerData _playerData;
        private DailyQuestController _dailyQuestController;
        private MainMenuController _mainMenuController;
        private Dictionary<EQuestCategory, List<Quest>> _quests;
        public bool Initialized { get; private set; }
        public Action<Quest> OnQuestCompleted;

        public DailyQuestsManager(ApplicationConfigs configs)
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
            _dailyQuestController.InitializeSlots(_quests);
            Initialized = true;
        }
        
        public void ClaimReward(Quest quest)
        {
            var data = quest.Config;
            quest.Model.State = EQuestState.Rewarded;
            ConsumeReward(data.RewardType, data.RewardAmount);
            SaveData();
        }

        private void LoadOrCreateData()
        {
            if(_quests != null)
                return;
            var save = _playerData.DailyQuests.Value;
            if (string.IsNullOrEmpty(save))
            {
                _quests = _generator.CreateNewQuests();
                SaveData();
            }
            else
            {
                var questsSave = JsonConvert.DeserializeObject<List<QuestModel>>(_playerData.DailyQuests.Value);
                _quests = new Dictionary<EQuestCategory, List<Quest>>();
                foreach (var pair in _config.Quests)
                {
                    var category = pair.Key;
                    if (!_quests.ContainsKey(category))
                        _quests[category] = new List<Quest>();
                }

                for (var i = 0; i < questsSave.Count; i++)
                {
                    var questModel = questsSave[i];
                    var data = _config.GetQuestData(questModel.Id);
                    var quest = new Quest(data, questModel, i);
                    _quests[data.Category].Add(quest);
                    quest.OnQuestCompleted += QuestCompleted;
                }
            }
        }

        private void QuestCompleted(Quest quest)
        {
            OnQuestCompleted?.Invoke(quest);
        }

        private void SaveData()
        {
            var saveData = (from pair in _quests from quest in pair.Value select quest.Model).ToList();
            _playerData.DailyQuests.Value = JsonConvert.SerializeObject(saveData);
        }

        private void ConsumeReward(ERewards type, int amount)
        {
            switch (type)
            {
                case ERewards.Acorn:
                    _playerData.Acorns.Value += amount;
                    break;
                case ERewards.Gem:
                    _playerData.Gems.Value += amount;
                    break;
            }
        }
    }
}