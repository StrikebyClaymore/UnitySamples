using Plugins.ServiceLocator;
using UISample.Features;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainSceneInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private MainMenuConfigs _configsContainer;
        [SerializeField] private UIContainer _uiContainer;
        public bool Initialized { get; private set; }

        private void Start()
        {
            Install();
            Initialize();
        }

        public override void Install()
        {
            ServiceLocator.ClearLocal();
            InstallDailyCalendar();
            InstallSceneUI();
        }

        public void Initialize()
        {
            var audioPlayer = ServiceLocator.Get<AudioPlayer>();
            audioPlayer.PlayMusic(audioPlayer.Config.MainMusicClip);
            ServiceLocator.GetLocal<DailyCalendarManager>().Initialize();
            ServiceLocator.Get<DailyQuestsManager>().Initialize();
            PlayerPrefs.Save();
            Initialized = true;
        }

        private void InstallDailyCalendar()
        {
            var dailyCalendar = new DailyCalendarManager(_configsContainer);
            ServiceLocator.RegisterLocal<DailyCalendarManager>(dailyCalendar);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(dailyCalendar);
        }

        private void InstallSceneUI()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            sceneUI.ClearControllers();
            sceneUI.RegisterController(typeof(MainMenuController), new MainMenuController(_uiContainer));
            sceneUI.RegisterController(typeof(SettingsController), new SettingsController(_uiContainer));
            sceneUI.RegisterController(typeof(DailyQuestController), new DailyQuestController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(DailyCalendarController), new DailyCalendarController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(PersonalController), new PersonalController(_uiContainer));
            sceneUI.RegisterController(typeof(SkinsController), new SkinsController(_uiContainer));
            sceneUI.RegisterController(typeof(ShopController), new ShopController(_uiContainer));
            sceneUI.RegisterController(typeof(LeaderboardController), new LeaderboardController(_uiContainer));
            sceneUI.ShowController<MainMenuController>();
        }
    }
}