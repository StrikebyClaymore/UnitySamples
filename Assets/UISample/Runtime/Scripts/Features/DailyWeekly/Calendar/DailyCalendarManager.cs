using System;
using Newtonsoft.Json;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;
using UISample.Utility;

namespace UISample.Features
{
    public class DailyCalendarManager : ILocalService, IInitializable, IUpdate, IDisposable
    {
        private readonly DailyCalendarConfig _config;
        private readonly PersistentTimer _timer;
        private readonly DailyRewardsGenerator _generator;
        private readonly PlayerData _playerData;
        private DailyCalendarController _dailyRewardController;
        private MainMenuController _mainMenuController;
        public PersistentTimer Timer => _timer;
        private DailyReward[] _rewards;
        public bool IsInitialized { get; private set; }
        
        public DailyCalendarManager(MainMenuConfigs configs)
        {
            _config = configs.DailyCalendarConfig;
            _timer = new PersistentTimer(PlayerDataConstants.DailyCalendarTimerKey, TimeSpan.FromSeconds(_config.TimerInterval));
            _timer.OnComplete += CalendarTimerCompleted;
            _generator = new DailyRewardsGenerator(_config);
            _playerData = ServiceLocator.Get<PlayerData>();
        }

        public void Initialize()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            _dailyRewardController = sceneUI.GetController<DailyCalendarController>();
            _mainMenuController = sceneUI.GetController<MainMenuController>();
            LoadOrCreateData();
            _timer.Start();
            IsInitialized = true;
        }

        public void CustomUpdate()
        {
            _timer?.CustomUpdate();
        }

        public void Dispose()
        {
            ServiceLocator.Get<ApplicationLoop>().RemoveUpdatable(this);
        }

        public void ClaimReward(int index)
        {
            var reward = _rewards[index];
            ConsumeReward((ERewards)reward.Id, reward.Amount);
            reward.State = EDailyRewardState.Rewarded;
            _playerData.LastRewardIndex.Value++;
            SaveData();
            _mainMenuController.SetCalendarNotification(false);
            _timer.Start(true);
        }

        private void LoadOrCreateData()
        {
            var save = _playerData.DailyCalendarRewards.Value;
            if (string.IsNullOrEmpty(save))
            {
                _rewards = _generator.GenerateRewards();
                SaveData();
            }
            else
            {
                _rewards = JsonConvert.DeserializeObject<DailyReward[]>(_playerData.DailyCalendarRewards.Value);
            }
            _dailyRewardController.InitializeSlots(_rewards);
        }

        private void SaveData()
        {
            _playerData.DailyCalendarRewards.Value = JsonConvert.SerializeObject(_rewards);
        }

        private void CalendarTimerCompleted()
        {
            var index = _playerData.LastRewardIndex.Value;
            if (_rewards[index].State is EDailyRewardState.Rewarded)
            {
                index++;
                _playerData.LastRewardIndex.Value = index;
            }
            _rewards[index].State = EDailyRewardState.Unlocked;
            SaveData();
            _dailyRewardController.UnlockSlot(index);
            _mainMenuController.SetCalendarNotification(true);
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