using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UnityEngine.SceneManagement;

namespace UISample.UI
{
    public class HollowController : BaseController
    {
        private readonly HollowView _view;
        
        public HollowController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<HollowView>();
            _view.Stop.onClick.AddListener(StopPressed);
            _view.Continue.onClick.AddListener(ContinuePressed);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }
        
        private void StopPressed()
        {
            ServiceLocator.Get<SceneLoader>().LoadSceneAsync(GameConstants.MainMenuSceneIndex, LoadSceneMode.Single);
        }

        private void ContinuePressed()
        {
            _sceneUI.HideController<HollowController>();
        }
    }
}