﻿using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private PlayerView _playerPrefab;
        [SerializeField] private PlayerConfig _playerConfig;
        
        public override void Install()
        {
            var mapGenerator = ServiceLocator.GetLocal<MapGenerator>();
            var playerView = Instantiate(_playerPrefab);
            var player = new PlayerController(playerView, _playerConfig, mapGenerator);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(player);
            _cameraFollow.Target = playerView.transform;
            mapGenerator.Target = playerView.transform;
        }
    }
}