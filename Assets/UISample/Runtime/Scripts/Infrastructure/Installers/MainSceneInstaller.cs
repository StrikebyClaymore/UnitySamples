using Plugins.ServiceLocator;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainSceneInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UIContainer _uiContainer;
        public bool Initialized { get; private set; }

        public override void Install()
        {
            InstallSceneUI();
        }

        public void Initialize()
        {
            ServiceLocator.Get<MainSceneUI>().Initialize();
            var audioPlayer = ServiceLocator.Get<AudioPlayer>();
            audioPlayer.PlayMusic(audioPlayer.Config.MainMusicClip);
            Initialized = true;
        }
        
        private void InstallSceneUI()
        {
            ServiceLocator.Register<MainSceneUI>(new MainSceneUI(_uiContainer));
        }
    }
}