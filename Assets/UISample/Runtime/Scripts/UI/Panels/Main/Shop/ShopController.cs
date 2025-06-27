using UISample.Data;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class ShopController : BaseController
    {
        private readonly ShopView _view;
        private readonly ShopConfig _config;

        public ShopController(UIContainer uiContainer, MainMenuConfigs configsContainer)
        {
            _view = uiContainer.GetView<ShopView>();
            _config = configsContainer.ShopConfig;
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

        public void InitializeSlots()
        {
            
        }

        private void ClosePressed()
        {
            _sceneUI.HideController<ShopController>();
        }
    }
}