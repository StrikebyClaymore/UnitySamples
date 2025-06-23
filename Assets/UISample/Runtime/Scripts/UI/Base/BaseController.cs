using System;
using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public abstract class BaseController : IDisposable
    {
        protected SceneUI _sceneUI;
        public bool Active { get; private set; }

        protected BaseController()
        {
            _sceneUI = ServiceLocator.Get<SceneUI>();
        }
        
        public virtual void Show(bool instantly = false)
        {
            Active = true;
        }

        public virtual void Hide(bool instantly = false)
        {
            Active = false;
        }

        public virtual void Dispose()
        {
            
        }
    }
}
