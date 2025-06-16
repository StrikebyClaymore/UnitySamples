using UISample.Infrastructure;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerController : IUpdatable
    {
        private readonly PlayerView _view;
        private readonly PlayerMovement _movement;
        private readonly PickupHandler _pickupHandler;
        
        public PlayerController(PlayerView view, MapGenerator mapGenerator)
        {
            _view = view;
            _movement = new PlayerMovement(_view, mapGenerator);
            _pickupHandler = new PickupHandler(mapGenerator);
            _view.OnTriggerEnter.AddListener(OnTriggerEnter);
        }

        public void CustomUpdate()
        {
            _movement.CustomUpdate();
        }

        public void CustomFixedUpdate() {
}

        public void CustomLateUpdate() { }
        
        private void OnTriggerEnter(Collider2D collider)
        {
            if (collider.TryGetComponent<PickupItem>(out var item))
            {
                _pickupHandler.Pickup(item);
            }
        }
    }
}