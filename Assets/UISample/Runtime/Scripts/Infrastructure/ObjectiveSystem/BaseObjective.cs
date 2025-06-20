using System;

namespace UISample.Infrastructure
{
    public abstract class BaseObjective : IDisposable
    {
        public bool Completed { get; private set; }
        public event Action OnComplete;

        public virtual void Dispose()
        {
            OnComplete = null;
            Completed = false;
            Unsubscribe();
        }

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();

        protected abstract void TryComplete();
        
        protected void Complete()
        {
            Completed = true;
            OnComplete?.Invoke();
        }
    }
}