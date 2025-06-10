using System.Collections;
using Plugins.ServiceLocator;
using UISample.Data;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class ApplicationInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private ConfigsContainer _configsContainer;
        public bool Initialized { get; private set; }

        public override void Install()
        {
            InstallSceneLoading();
            InstallPlayerData();
            InstallApplicationLoop();
            InstallAudioSettings();
            InstallAudioPlayer();
            InstallSceneUI();
        }

        public void Initialize()
        {
            ServiceLocator.Get<PlayerData>().Initialize();
            ServiceLocator.Get<AudioSettings>().Initialize();
            ServiceLocator.Get<LoadingSceneUI>().Initialize();
            Initialized = true;
            StartCoroutine(LoadMainScene());
        }

        private IEnumerator LoadMainScene()
        {
            var sceneLoader = ServiceLocator.Get<SceneLoader>();
            yield return sceneLoader.LoadSceneAsync(GameConstants.MainMenuSceneIndex);
            var mainSceneInstaller = GameObject.FindObjectOfType<MainSceneInstaller>();
            mainSceneInstaller.Install();
            mainSceneInstaller.Initialize();
            sceneLoader.UnloadSceneAsync(GameConstants.LoadingSceneIndex);
        }

        private void InstallSceneLoading()
        {
            ServiceLocator.Register<SceneLoader>(new SceneLoader());
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
            ServiceLocator.Register<LoadingSceneUI>(new LoadingSceneUI(_uiContainer));
        }
    }
}