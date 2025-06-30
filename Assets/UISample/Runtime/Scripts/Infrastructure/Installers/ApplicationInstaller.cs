using System.Collections;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class ApplicationInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private ApplicationConfigs _configsContainer;
        public bool IsInitialized { get; private set; }

        public override void Install()
        {
            ServiceLocator.ClearLocal();
            InstallSceneLoading();
            InstallPlayerData();
            InstallGameplayData();
            InstallApplicationLoop();
            InstallAudioSettings();
            InstallAudioPlayer();
            InstallSceneUI();
            InstallQuestsManager();
            InstallAdvManager();
            InstallPurchaseManager();
        }

        public void Initialize()
        {
            ServiceLocator.Get<PlayerData>().Initialize();
            ServiceLocator.Get<AudioSettings>().Initialize();
            ServiceLocator.Get<SceneUI>().Initialize();
            IsInitialized = true;
            StartCoroutine(LoadMainScene());
        }

        private IEnumerator LoadMainScene()
        {
            var sceneLoader = ServiceLocator.Get<SceneLoader>();
            yield return sceneLoader.LoadSceneAsync(GameConstants.MainMenuSceneIndex);
            sceneLoader.UnloadSceneAsync(GameConstants.LoadingSceneIndex);
        }
        
        private void InstallPurchaseManager()
        {
            ServiceLocator.Register(new StubPurchaseManager());
        }

        private void InstallAdvManager()
        {
            ServiceLocator.Register(new StubAdvManager());
        }
        
        private void InstallQuestsManager()
        {
            var dailyQuestsManager = new DailyQuestsManager(_configsContainer);
            ServiceLocator.Register<DailyQuestsManager>(dailyQuestsManager);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(dailyQuestsManager);
        }

        private void InstallSceneLoading()
        {
            ServiceLocator.Register<SceneLoader>(new SceneLoader());
        }

        private void InstallGameplayData()
        {
            ServiceLocator.Register<GameplayData>(new GameplayData());
        }
        
        private void InstallPlayerData()
        {
            var defaultSettings = new PlayerDataDefaultSettings()
            {
                SoundVolume = _configsContainer.AudioConfig.DefaultSoundVolume,
                MusicVolume = _configsContainer.AudioConfig.DefaultMusicVolume,
                UIVolume = _configsContainer.AudioConfig.DefaultUIVolume,
            };
            ServiceLocator.Register<PlayerData>(new PlayerData(defaultSettings));
        }
        
        private void InstallApplicationLoop()
        {
            var applicationLoop = new GameObject(nameof(ApplicationLoop)).AddComponent<ApplicationLoop>();
            ServiceLocator.Register<ApplicationLoop>(applicationLoop);
        }
        
        private void InstallAudioPlayer()
        {
            ServiceLocator.Register<AudioPlayer>(new AudioPlayer(_configsContainer));
        }
        
        private void InstallAudioSettings()
        {
            ServiceLocator.Register<AudioSettings>(new AudioSettings());
        }
        
        private void InstallSceneUI()
        {
            var sceneUI = new SceneUI();
            ServiceLocator.Register<SceneUI>(sceneUI);
            sceneUI.RegisterController(typeof(LoadingController), new LoadingController(_uiContainer));
            sceneUI.ShowController<LoadingController>();
        }
    }
}