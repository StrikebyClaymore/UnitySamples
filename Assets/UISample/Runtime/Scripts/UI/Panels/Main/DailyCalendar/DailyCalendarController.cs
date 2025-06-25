using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;
using UISample.Utility;

namespace UISample.UI
{
    public class DailyCalendarController : BaseController
    {
        private readonly DailyCalendarView _view;
        private readonly DailyCalendarConfig _config;
        private readonly DailyCalendarManager _calendarManager;
        private readonly AudioPlayer _audioPlayer;
        
        public DailyCalendarController(UIContainer uiContainer, MainMenuConfigs configs)
        {
            _view = uiContainer.GetView<DailyCalendarView>();
            _config = configs.DailyCalendarConfig;
            _calendarManager = ServiceLocator.GetLocal<DailyCalendarManager>();
            _audioPlayer = ServiceLocator.Get<AudioPlayer>();
            _calendarManager.Timer.OnUpdate += UpdateCalendarTime;
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
        }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }
        
        private void ClosePressed()
        {
            _sceneUI.HideController<DailyCalendarController>();
        }

        private void UpdateCalendarTime(TimeSpan time)
        {
            _view.TimerText.SetText(time.ToHHMMSS());
        }

        public void InitializeSlots(DailyReward[] rewards)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                var item = rewards[i];
                var data = _config.GetData(item.Id);
                var slot = _view.Slots[i];
                slot.Initialize(item, data, i, SlotPressed);
            }
        }

        public void UnlockSlot(int index)
        {
            var slot = _view.Slots[index];
            slot.SetState(EDailyRewardState.Unlocked);
        }

        private void SlotPressed(int index)
        {
            var slot = _view.Slots[index];
            slot.SetState(EDailyRewardState.Rewarded);
            _calendarManager.ClaimReward(index);
            _audioPlayer.PlayUI(_audioPlayer.Config.UIPickupClip);
        }
    }
}