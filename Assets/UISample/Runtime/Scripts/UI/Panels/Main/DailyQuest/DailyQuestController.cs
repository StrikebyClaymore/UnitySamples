using UISample.Infrastructure;

namespace UISample.UI
{
    public class DailyQuestController : BaseController
    {
        private readonly DailyQuestView _view;

        public DailyQuestController(UIContainer uiContainer, MainMenuConfigs configs)
        {
            _view = uiContainer.GetView<DailyQuestView>();
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
            _sceneUI.HideController<DailyQuestController>();
        }

        public void InitializeSlots()
        {
            throw new System.NotImplementedException();
        }
    }
}