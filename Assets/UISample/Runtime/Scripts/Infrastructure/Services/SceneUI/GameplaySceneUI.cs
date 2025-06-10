using UISample.UI;

namespace UISample.Infrastructure
{
    public class GameplaySceneUI : SceneUI
    {
        public GameplaySceneUI(UIContainer uiContainer) : base(uiContainer)
        {
            
        }
        
        public override void Initialize()
        {
            CreateControllers(_uiContainer);
            ShowController<ControlsController>();
        }
        protected sealed override void CreateControllers(UIContainer uiContainer)
        {
            _controllers.Add(typeof(ControlsController), new ControlsController(uiContainer));
        }
    }
}