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
            foreach (var controller in _controllers.Values)
                controller.Hide(true);
            ShowController<ControlsController>();
        }
        protected sealed override void CreateControllers(UIContainer uiContainer)
        {
            _controllers.Add(typeof(ControlsController), new ControlsController(uiContainer));
            _controllers.Add(typeof(HollowController), new HollowController(uiContainer));
        }
    }
}