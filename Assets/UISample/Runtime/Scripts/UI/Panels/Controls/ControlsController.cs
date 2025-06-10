using UISample.Infrastructure;

namespace UISample.UI
{
    public class ControlsController : BaseController
    {
        private readonly ControlsView _view;
        
        public ControlsController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<ControlsView>();
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
    }
}