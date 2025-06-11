using DG.Tweening;
using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UISample.UI;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerMovement : IUpdate
    {
        private readonly Transform _transform;
        private bool _isMoving = false;
        
        public PlayerMovement(PlayerView view)
        {
            _transform = view.transform;
            var controls = ServiceLocator.Get<GameplaySceneUI>().GetController<ControlsController>();
            controls.OnControlPressed.AddListener(HandleControlPressed);
        }

        public void CustomUpdate()
        {
            
        }

        private void HandleControlPressed(Vector2 direction)
        {
            if(_isMoving)
                return;
            _isMoving = true;
            Vector3 displacement = direction * 4f;
            _transform.DOMove(_transform.position + displacement, 0.5f).OnComplete(() =>
            {
                _isMoving = false;
            });
        }
    }
}