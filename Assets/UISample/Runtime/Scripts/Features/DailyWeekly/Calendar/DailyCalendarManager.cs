using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.Utility;

namespace UISample.Features
{
    public class DailyCalendarManager : ILocalService, IInitializable, IUpdate, IDisposable
    {
        private readonly DailyCalendarConfig _config;
        private readonly PersistentTimer _timer;
        public PersistentTimer Timer => _timer;
        public bool Initialized { get; private set; }
        
        public DailyCalendarManager(MainMenuConfigs configs)
        {
            _config = configs.DailyCalendarConfig;
            _timer = new PersistentTimer(_config.TimerKey, _config.TimerInterval);
        }
        
        public void Initialize()
        {
            _timer.Start();
            Initialized = true;
        }
        
        public void CustomUpdate()
        {
            _timer?.CustomUpdate();
        }

        public void Dispose()
        {
            ServiceLocator.Get<ApplicationLoop>().RemoveUpdatable(this);
        }
    }
}