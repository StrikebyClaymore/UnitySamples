using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private PlayerView _playerPrefab;
        
        public override void Install()
        {
            var playerView = Instantiate(_playerPrefab);
            var player = new PlayerController(playerView);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(player);
            _cameraFollow.Target = playerView.transform;
        }
    }
}