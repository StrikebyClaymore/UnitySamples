using System;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class AcceptPopup : Popup
    {
        private readonly AcceptPopupView _view;
        public event Action OnAccept;
        public event Action OnCancel;
        public event Action<bool> OnClose;

        public AcceptPopup(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<AcceptPopupView>();
            _view.CloseButton.onClick.AddListener(Cancel);
            _view.ShadowCloseButton.onClick.AddListener(Cancel);
            _view.AcceptButton.onClick.AddListener(Accept);
            _view.CancelButton.onClick.AddListener(Cancel);
        }

        public void Show(string message, bool instantly = false)
        {
            _view.MessageText.SetText(message);
            Show(instantly);
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

        private void Accept()
        {
            OnAccept?.Invoke();
            OnClose?.Invoke(true);
            Hide();
        }

        private void Cancel()
        {
            OnCancel?.Invoke();
            OnClose?.Invoke(false);
            Hide();
        }
    }
}