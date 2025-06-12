using Plugins.ServiceLocator;
using UISample.Features;
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
        }

        public override void Install()
        {
            InstallSceneUI();
            InstallParallax();
            InstallCameraFollow();
            Initialize();
        }
        
        public void Initialize()
        {
            _mapGenerator.Initialize();
            ServiceLocator.Get<GameplaySceneUI>().Initialize();
            _playerInstaller.Install();
            Initialized = true;
        }

        private void InstallSceneUI()
        {
            ServiceLocator.Register<GameplaySceneUI>(new GameplaySceneUI(_uiContainer));
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