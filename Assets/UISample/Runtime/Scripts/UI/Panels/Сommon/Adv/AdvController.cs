using UISample.Infrastructure;

namespace UISample.UI
{
    public class AdvController : BaseController
    {
        private readonly AdvView _view;

        public AdvController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<AdvView>();
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
            _sceneUI.HideController<DailyCalendarController>();
        }
    }
}