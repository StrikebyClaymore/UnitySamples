using Plugins.ServiceLocator;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private ConfigsContainer _configsContainer;

        public override void Install()
        {
            InstallPlayerData();
            InstallCoroutineHelper();
            InstallApplicationLoop();
            InstallSceneUI();
            InstallAudioSettings();
            InstallAudioPlayer();
        }

        public void Initialize()
        {
            ServiceLocator.Get<PlayerData>().Initialize();
            ServiceLocator.Get<SceneUI>().Initialize();
            ServiceLocator.Get<AudioPlayer>().Initialize();
            ServiceLocator.Get<AudioSettings>().Initialize();
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
        
        private void InstallCoroutineHelper()
        {
            var coroutineHelper = new GameObject(nameof(CoroutineHelper)).AddComponent<CoroutineHelper>();
            ServiceLocator.Register<CoroutineHelper>(coroutineHelper);
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
            ServiceLocator.Register<MainSceneUI>(new MainSceneUI(_uiContainer));
        }
    }
}