using UISample.Infrastructure;

namespace UISample.UI
{
    public class DailyCalendarController : BaseController
    {
        private readonly DailyCalendarView _view;

        public DailyCalendarController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<DailyCalendarView>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
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
            _sceneUI.HideController<DailyCalendarController>();
        }
    }
}