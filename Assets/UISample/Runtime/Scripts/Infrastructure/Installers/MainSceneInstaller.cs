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
        [SerializeField] private Canvas _rootCanvas;
        public bool IsInitialized { get; private set; }

        private void Start()
        {
            Install();
            Initialize();
        }

        public override void Install()
        {
            ServiceLocator.ClearLocal();
            InstallDailyCalendar();
            InstallSkins();
            InstallShop();
            InstallSceneUI();
        }

        public void Initialize()
        {
            var audioPlayer = ServiceLocator.Get<AudioPlayer>();
            audioPlayer.PlayMusic(audioPlayer.Config.MainMusicClip);
            ServiceLocator.GetLocal<DailyCalendarManager>().Initialize();
            ServiceLocator.GetLocal<SkinsManager>().Initialize();
            ServiceLocator.GetLocal<Shop>().Initialize();
            ServiceLocator.Get<DailyQuestsManager>().Initialize();
            PlayerPrefs.Save();
            IsInitialized = true;
        }

        private void InstallDailyCalendar()
        {
            var dailyCalendar = new DailyCalendarManager(_configsContainer);
            ServiceLocator.RegisterLocal<DailyCalendarManager>(dailyCalendar);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(dailyCalendar);
        }

        private void InstallSkins()
        {
            ServiceLocator.RegisterLocal<SkinsManager>(new SkinsManager(_configsContainer));
        }

        private void InstallShop()
        {
            ServiceLocator.RegisterLocal<Shop>(new Shop(_configsContainer));
        }

        private void InstallSceneUI()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            sceneUI.RootCanvas = _rootCanvas;
            sceneUI.ClearControllers();
            sceneUI.RegisterController(typeof(MainMenuController), new MainMenuController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(SettingsController), new SettingsController(_uiContainer));
            sceneUI.RegisterController(typeof(DailyQuestController), new DailyQuestController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(DailyCalendarController), new DailyCalendarController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(PersonalController), new PersonalController(_uiContainer));
            sceneUI.RegisterController(typeof(SkinsController), new SkinsController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(ShopController), new ShopController(_uiContainer, _configsContainer));
            sceneUI.RegisterController(typeof(LeaderboardController), new LeaderboardController(_uiContainer));
            sceneUI.RegisterController(typeof(RouletteController), new RouletteController(_uiContainer));
            sceneUI.RegisterController(typeof(AcceptPopup), new AcceptPopup(_uiContainer));
            sceneUI.ShowController<MainMenuController>();
        }
    }
}