using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;
using UISample.Utility;
using UnityEngine.SceneManagement;

namespace UISample.UI
{
    public class MainMenuController : BaseController
    {
        private readonly MainMenuView _view;
        
        public MainMenuController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<MainMenuView>();
            _view.PlayButton.onClick.AddListener(PlayPressed);
            _view.PersonalButton.onClick.AddListener(PersonalPressed);
            _view.SkinsButton.onClick.AddListener(SkinsPressed);
            _view.ShopButton.onClick.AddListener(ShopPressed);
            _view.LeaderboardButton.onClick.AddListener(LeaderboardPressed);
            _view.SettingsButton.onClick.AddListener(SettingsPressed);
            var playerData = ServiceLocator.Get<PlayerData>();
            playerData.Acorns.OnValueChanged += UpdateAcorns;
            playerData.Gems.OnValueChanged += UpdateGems;
            UpdateAcorns(playerData.Acorns.Value);
            UpdateGems(playerData.Gems.Value);
            _view.DailyCalendarNotification.Hide();
            _view.DailyQuestsNotification.Hide();
            var dailyCalendar = ServiceLocator.GetLocal<DailyCalendarManager>();
            dailyCalendar.Timer.OnUpdate += UpdateCalendarTime;
            _view.DailyQuestsButton.onClick.AddListener(DailyQuestsPressed);
            _view.DailyCalendarButton.onClick.AddListener(DailyCalendarPressed);
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

        public void SetCalendarNotification(bool enable)
        {
            _view.DailyCalendarNotification.SetActive(enable);
        }

        public void SetQuestNotification(bool enable)
        {
            _view.DailyQuestsNotification.SetActive(enable);
        }

        private void PlayPressed()
        {
            var sceneLoader = ServiceLocator.Get<SceneLoader>();
            sceneLoader.LoadSceneAsync(GameConstants.GameplaySceneIndex, LoadSceneMode.Single);
        }

        private void PersonalPressed()
        {
            _sceneUI.ShowController<PersonalController>();
        }

        private void SkinsPressed()
        {
            _sceneUI.ShowController<SkinsController>();
        }

        private void ShopPressed()
        {
            _sceneUI.ShowController<ShopController>();
        }

        private void LeaderboardPressed()
        {
            _sceneUI.ShowController<LeaderboardController>();
        }
        
        private void SettingsPressed()
        {
            _sceneUI.ShowController<SettingsController>();
        }

        private void UpdateAcorns(int value)
        {
            _view.AcornsText.text = value.ToString();
        }

        private void UpdateGems(int value)
        {
            _view.GemsText.text = value.ToString();
        }

        private void UpdateCalendarTime(TimeSpan time)
        {
            _view.DailyCalendarTimerText.SetText(time.ToHHMMSS());
        }

        private void DailyQuestsPressed()
        {
            _sceneUI.ShowController<DailyQuestController>();
        }

        private void DailyCalendarPressed()
        {
            _sceneUI.ShowController<DailyCalendarController>();
        }
    }
}
