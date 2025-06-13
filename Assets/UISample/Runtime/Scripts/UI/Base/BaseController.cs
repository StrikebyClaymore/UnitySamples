using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public abstract class BaseController
    {
        protected SceneUI _sceneUI;

        protected BaseController()
        {
            _sceneUI = ServiceLocator.Get<SceneUI>();
        }
        
        public virtual void Show(bool instantly = false)
        {
            
        }

        public virtual void Hide(bool instantly = false)
        {
            
        }
    }
}
