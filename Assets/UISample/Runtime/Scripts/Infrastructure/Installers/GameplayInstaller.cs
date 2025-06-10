using Plugins.ServiceLocator;
using UISample.Features;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private PlayerInstaller _playerInstaller;

        private void Start()
        {
            Install();
        }

        public override void Install()
        {
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(_cameraFollow);
            _playerInstaller.Install();
        }
    }
}