using UISample.Infrastructure;

namespace UISample.UI
{
    public class ShopController : BaseController
    {
        private readonly ShopView _view;

        public ShopController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<ShopView>();
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
            _sceneUI.HideController<ShopController>();
        }
    }
}