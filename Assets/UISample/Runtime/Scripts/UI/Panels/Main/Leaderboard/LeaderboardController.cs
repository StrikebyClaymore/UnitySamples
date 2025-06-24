using UISample.Infrastructure;

namespace UISample.UI
{
    public class LeaderboardController : BaseController
    {
        private readonly LeaderboardView _view;

        public LeaderboardController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<LeaderboardView>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
        
        private void ClosePressed()
        {
            _sceneUI.HideController<LeaderboardController>();
        }
    }
}