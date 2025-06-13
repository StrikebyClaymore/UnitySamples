using Plugins.ServiceLocator;
using UISample.Features;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private Parallax _parallax;
        [SerializeField] private PlayerInstaller _playerInstaller;
        [SerializeField] private MapGeneratorMono _mapGenerator;
        public bool Initialized { get; private set; }

        private void Start()
        {
            Install();
            Initialize();
        }

        public override void Install()
        {
            InstallSceneUI();
            InstallParallax();
            InstallCameraFollow();
        }

        public void Initialize()
        {
            ServiceLocator.Get<GameplayData>().ResetData();
            _mapGenerator.Initialize();
            _playerInstaller.Install();
            Initialized = true;
        }

        private void InstallSceneUI()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            sceneUI.ClearControllers();
            sceneUI.RegisterController(typeof(ControlsController), new ControlsController(_uiContainer));
            sceneUI.RegisterController(typeof(HollowController), new HollowController(_uiContainer));
            sceneUI.RegisterController(typeof(HUDController), new HUDController(_uiContainer));
            sceneUI.ShowController<ControlsController>();
            sceneUI.ShowController<HUDController>();
        }
        
        private void InstallParallax()
        {
            _parallax.Initialize();
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(_parallax);
        }
        
        private void InstallCameraFollow()
        {
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(_cameraFollow);
        }
    }
}