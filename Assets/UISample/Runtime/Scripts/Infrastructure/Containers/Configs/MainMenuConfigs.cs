using UISample.Data;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainMenuConfigs : MonoBehaviour
    {
        [field: SerializeField] public DailyCalendarConfig DailyCalendarConfig { get; private set; }
        [field: SerializeField] public DailyQuestsConfig DailyQuestsConfig { get; private set; }
        [field: SerializeField] public SkinsConfig SkinsConfig { get; private set; }
        [field: SerializeField] public ShopConfig ShopConfig { get; private set; }
    }
}