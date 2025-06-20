using UISample.Data;

namespace UISample.Features
{
    public class QuestsGenerator
    {
        private readonly DailyQuestsConfig _config;
        
        public QuestsGenerator(DailyQuestsConfig config)
        {
            _config = config;
        }
    }
}