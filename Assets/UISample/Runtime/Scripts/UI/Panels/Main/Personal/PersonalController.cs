using UISample.Infrastructure;

namespace UISample.UI
{
    public class PersonalController : BaseController
    {
        private readonly PersonalView _view;

        public PersonalController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<PersonalView>();
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