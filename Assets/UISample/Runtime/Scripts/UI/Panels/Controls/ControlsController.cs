using UISample.Infrastructure;
using UnityEngine;
using UnityEngine.Events;

namespace UISample.UI
{
    public class ControlsController : BaseController
    {
        private readonly ControlsView _view;
        public readonly UnityEvent<Vector2> OnControlPressed = new();
        
        public ControlsController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<ControlsView>();
            _view.TopButton.onClick.AddListener(() => ControlPressed(Vector2.up));
            _view.RightButton.onClick.AddListener(() => ControlPressed(Vector2.right));
            _view.DownButton.onClick.AddListener(() => ControlPressed(Vector2.down));
            _view.LeftButton.onClick.AddListener(() => ControlPressed(Vector2.left));
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
        
        private void ControlPressed(Vector2 direction)
        {
            OnControlPressed?.Invoke(direction);
        }
    }
}