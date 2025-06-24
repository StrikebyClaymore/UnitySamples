using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;
using UISample.Utility;

namespace UISample.Features
{
    public class DailyQuestsManager : IService, IInitializable, IUpdate
    {
        private readonly DailyQuestsConfig _config;
        private readonly QuestsGenerator _generator;
        private readonly PlayerData _playerData;
        private readonly PersistentTimer _dailyTimer;
        private readonly PersistentTimer _weeklyTimer;
        private readonly List<Quest> _waitClaimQuests = new();
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
            _dailyTimer = new PersistentTimer(PlayerDataConstants.DailyQuestTimerKey, TimeSpan.FromDays(1));
            _weeklyTimer = new PersistentTimer(PlayerDataConstants.WeeklyQuestTimerKey, TimeSpan.FromDays(7));
            LoadOrCreateData();
            _dailyTimer.OnComplete += ResetDailyQuests;
            _weeklyTimer.OnComplete += ResetWeeklyQuests;
            _dailyTimer.Start();
            _weeklyTimer.Start();
        }

        public void Initialize()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            _dailyQuestController = sceneUI.GetController<DailyQuestController>();
            _mainMenuController = sceneUI.GetController<MainMenuController>();
            if(_waitClaimQuests.Count > 0)
                _mainMenuController.SetQuestNotification(true);
            _dailyQuestController.InitializeSlots(_quests);
            Initialized = true;
        }

        public void CustomUpdate()
        {
            _dailyTimer?.CustomUpdate();
            _weeklyTimer?.CustomUpdate();
        }

        public void ClaimReward(Quest quest)
        {
            var data = quest.Config;
            quest.Model.State = EQuestState.Rewarded;
            ConsumeReward(data.RewardType, data.RewardAmount);
            SaveData();
            _waitClaimQuests.Remove(quest);
            if(_waitClaimQuests.Count == 0)
                _mainMenuController.SetQuestNotification(false);
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
                }
            }

            foreach (var quest in _quests.Values.SelectMany(quests => quests))
            {
                if (quest.Model.State is EQuestState.Completed)
                    _waitClaimQuests.Add(quest);
                else if(quest.Model.State is EQuestState.Process)
                    quest.OnQuestCompleted += QuestCompleted;
                else
                    quest.Dispose();
            }
        }

        private void QuestCompleted(Quest quest)
        {
            OnQuestCompleted?.Invoke(quest);
            quest.Model.State = EQuestState.Completed;
            quest.Dispose();
            _waitClaimQuests.Add(quest);
            SaveData();
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

        private void ResetDailyQuests()
        {
            foreach (var quest in _quests[EQuestCategory.Daily])
                quest.Dispose();
            _quests[EQuestCategory.Daily] = _generator.CreateQuestsForCategory(EQuestCategory.Daily);
            foreach (var quest in _quests[EQuestCategory.Daily])
                quest.OnQuestCompleted += QuestCompleted;
            SaveData();
            _dailyTimer.Start(true);
            if(_dailyQuestController is { Active: true })
                _dailyQuestController.Show();
        }

        private void ResetWeeklyQuests()
        {
            foreach (var quest in _quests[EQuestCategory.Weekly])
                quest.Dispose();
            _quests[EQuestCategory.Weekly] = _generator.CreateQuestsForCategory(EQuestCategory.Weekly);
            foreach (var quest in _quests[EQuestCategory.Weekly])
                quest.OnQuestCompleted += QuestCompleted;
            SaveData();
            _weeklyTimer.Start(true);
            if(_dailyQuestController is { Active: true })
                _dailyQuestController.Show();
        }
    }
}