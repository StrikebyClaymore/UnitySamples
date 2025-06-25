using Plugins.ServiceLocator;
using UISample.Data;

namespace UISample.Features
{
    public class SkinsManager : IService
    {
        private SkinsConfig _config;
        
        public SkinsManager(SkinsConfig config)
        {
            _config = config;
        }
    }
}