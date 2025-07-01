using System;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class AdvController : BaseController
    {
        private readonly AdvView _view;
        public event Action<bool> OnChosen;

        public AdvController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<AdvView>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
            _view.AcceptButton.onClick.AddListener(() => Choice(true));
            _view.CancelButton.onClick.AddListener(() => Choice(false));
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
            Hide();
            OnChosen?.Invoke(false);
        }

        private void Choice(bool value)
        {
            Hide();
            OnChosen?.Invoke(value);
        }
    }
}