using System;

namespace UISample.Infrastructure
{
    public abstract class BaseObjective : IObjective
    {
        public bool IsCompleted { get; protected set; }
        public event Action OnUpdated;
        public event Action OnCompleted;

        public virtual void Dispose()
        {
            OnUpdated = null;
            OnCompleted = null;
            IsCompleted = false;
            Unsubscribe();
        }

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();

        protected abstract void TryComplete();
        
        protected virtual void Update()
        {
            OnUpdated?.Invoke();
            TryComplete();
        }
        
        protected virtual void Complete()
        {
            IsCompleted = true;
            OnCompleted?.Invoke();
            Unsubscribe();
        }
    }
    
    public abstract class BaseObjective<T> : BaseObjective, IObjective<T> where T : struct
    {
        public T Target { get; }
        public T Progress { get; protected set; }
        public event Action<T> OnProgressChanged;

        protected BaseObjective(T target, T progress = default)
        {
            Target = target;
            Progress = progress;
        }
        
        public override void Dispose()
        {
            OnProgressChanged = null;
            base.Dispose();
        }
        
        protected override void Update()
        {
            OnProgressChanged?.Invoke(Progress);
            base.Update();
        }
    }
}