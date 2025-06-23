using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class LoadingController : BaseController
    {
        private readonly LoadingView _view;
        
        public LoadingController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<LoadingView>();
            var sceneLoader = ServiceLocator.Get<SceneLoader>();
            sceneLoader.OnLoadingUpdate += UpdateLoadingProgress;
            _view.ProgressSlider.value = 0;
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

        private void UpdateLoadingProgress(float value)
        {
            _view.ProgressSlider.value = value;
        }
    }
}