using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class HUDController : BaseController
    {
        private readonly HUDView _view;
        
        public HUDController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<HUDView>();
            var gameplayData = ServiceLocator.Get<GameplayData>();
            gameplayData.Acorns.OnValueChanged += UpdateAcorns;
            UpdateAcorns(gameplayData.Acorns.Value);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
        
        private void UpdateAcorns(int value)
        {
            _view.AcornsText.text = value.ToString();
        }
    }
}