using System;

namespace UISample.Infrastructure
{
    public interface IObjective : IDisposable
    {
        bool IsCompleted { get; }
        event Action OnUpdated;
        event Action OnCompleted;
    }
    
    public interface IObjective<T> : IObjective where T : struct
    {
        T Progress { get; }
        T Target { get; }
        event Action<T> OnProgressChanged;
    }
}