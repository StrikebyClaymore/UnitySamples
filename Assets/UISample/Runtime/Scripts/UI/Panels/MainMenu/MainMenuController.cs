using UISample.Infrastructure;

namespace UISample.UI
{
    public class MainMenuController : BaseController
    {
        private readonly MainMenuView _view;
        
        
        public MainMenuController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<MainMenuView>();
            _view.SettingsButton.onClick.AddListener(SettingsPressed);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
        
        private void SettingsPressed()
        {
            _sceneUI.ShowController<SettingsController>();
        }
    }
}