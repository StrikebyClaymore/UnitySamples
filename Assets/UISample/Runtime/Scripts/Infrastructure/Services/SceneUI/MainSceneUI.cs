using UISample.UI;

namespace UISample.Infrastructure
{
    public class MainSceneUI : SceneUI
    {
        public MainSceneUI(UIContainer uiContainer) : base(uiContainer)
        {
        }

        public override void Initialize()
        {
            CreateControllers(_uiContainer);
            foreach (var controller in _controllers.Values)
                controller.Hide(true);
            ShowController<MainMenuController>();
        }

        protected sealed override void CreateControllers(UIContainer uiContainer)
        {
            _controllers.Add(typeof(MainMenuController), new MainMenuController(uiContainer));
            _controllers.Add(typeof(SettingsController), new SettingsController(uiContainer));
        }
    }
}