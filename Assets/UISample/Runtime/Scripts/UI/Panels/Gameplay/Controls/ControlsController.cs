using UISample.Infrastructure;
using UnityEngine;
using UnityEngine.Events;

namespace UISample.UI
{
    public class ControlsController : BaseController
    {
        private readonly ControlsView _view;
        public readonly UnityEvent<Vector3Int> OnControlPressed = new();
        public readonly UnityEvent<Vector3Int> OnControlReleased = new();
        
        public ControlsController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<ControlsView>();
            _view.TopButton.OnButtonPress.AddListener(() => ControlPressed(Vector3Int.up));
            _view.RightButton.OnButtonPress.AddListener(() => ControlPressed(Vector3Int.right));
            _view.DownButton.OnButtonPress.AddListener(() => ControlPressed(Vector3Int.down));
            _view.LeftButton.OnButtonPress.AddListener(() => ControlPressed(Vector3Int.left));   
            
            _view.TopButton.OnButtonRelease.AddListener(() => ControlReleased(Vector3Int.up));
            _view.RightButton.OnButtonRelease.AddListener(() => ControlReleased(Vector3Int.right));
            _view.DownButton.OnButtonRelease.AddListener(() => ControlReleased(Vector3Int.down));
            _view.LeftButton.OnButtonRelease.AddListener(() => ControlReleased(Vector3Int.left));
        }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }
        
        private void ControlPressed(Vector3Int direction)
        {
            OnControlPressed?.Invoke(direction);
        }
        
        private void ControlReleased(Vector3Int direction)
        {
            OnControlReleased?.Invoke(direction);
        }
    }
}