using UISample.Infrastructure;

namespace UISample.UI
{
    public class RouletteController : BaseController
    {
        private readonly RouletteView _view;

        public RouletteController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<RouletteView>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
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
        
        private void ClosePressed()
        {
            _sceneUI.HideController<PersonalController>();
        }
    }
}