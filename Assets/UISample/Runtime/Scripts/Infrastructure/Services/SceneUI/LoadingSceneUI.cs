using UISample.UI;

namespace UISample.Infrastructure
{
    public class LoadingSceneUI : SceneUI
    {
        public LoadingSceneUI(UIContainer uiContainer) : base(uiContainer)
        {
  
        }
        
        public override void Initialize()
        {
            CreateControllers(_uiContainer);
            ShowController<LoadingController>();
        }

        protected sealed override void CreateControllers(UIContainer uiContainer)
        {
            _controllers.Add(typeof(LoadingController), new LoadingController(uiContainer));
        }
    }
}