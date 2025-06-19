using Plugins.ServiceLocator;
using UISample.Features;
using UISample.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UISample.Infrastructure
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private GameplayConfigs _configsContainer;
        [SerializeField] private UIContainer _uiContainer;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private Parallax _parallax;
        [SerializeField] private PlayerInstaller _playerInstaller;
        [SerializeField] private Tilemap _tilemap;
        public bool Initialized { get; private set; }

        private void Start()
        {
            Install();
            Initialize();
        }

        public override void Install()
        {
            ServiceLocator.ClearLocal();
            InstallSceneUI();
            InstallParallax();
            InstallCameraFollow();
            InstallMapGenerator();
        }

        public void Initialize()
        {
            ServiceLocator.Get<GameplayData>().ResetData();
            ServiceLocator.GetLocal<MapGenerator>().Initialize();
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
            sceneUI.RegisterController(typeof(PauseController), new PauseController(_uiContainer));
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
        
        private void InstallMapGenerator()
        {
            var mapGenerator = new MapGenerator(_configsContainer, _tilemap);
            ServiceLocator.RegisterLocal<MapGenerator>(mapGenerator);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(mapGenerator);
        }
    }
}