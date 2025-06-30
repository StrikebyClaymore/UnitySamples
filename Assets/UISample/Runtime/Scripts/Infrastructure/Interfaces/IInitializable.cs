namespace UISample.Infrastructure
{
    public interface IInitializable
    {
        public bool IsInitialized { get; }

        public void Initialize();
    }
}