using Plugins.ServiceLocator;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainSceneInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UIContainer _uiContainer;
        public bool Initialized { get; private set; }

        private void Start()
        {
            Install();
            Initialize();
        }

        public override void Install()
        {
            InstallSceneUI();
        }

        public void Initialize()
        {
            var audioPlayer = ServiceLocator.Get<AudioPlayer>();
            audioPlayer.PlayMusic(audioPlayer.Config.MainMusicClip);
            Initialized = true;
        }
        
        private void InstallSceneUI()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            sceneUI.ClearControllers();
            sceneUI.RegisterController(typeof(MainMenuController), new MainMenuController(_uiContainer));
            sceneUI.RegisterController(typeof(SettingsController), new SettingsController(_uiContainer));
            sceneUI.ShowController<MainMenuController>();
        }
    }
}