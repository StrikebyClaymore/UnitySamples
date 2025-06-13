using Plugins.ServiceLocator;
using UISample.Utilities;

namespace UISample.Infrastructure
{
    public class GameplayData : IService
    {
        public ReactiveProperty<int> Acorns { get; private set; } = new();

        public GameplayData()
        {
            
        }

        public void ResetData()
        {
            Acorns.Value = 0;
        }
    }
}