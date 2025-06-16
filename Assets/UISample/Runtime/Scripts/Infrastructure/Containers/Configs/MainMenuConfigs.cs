using UISample.Data;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class MainMenuConfigs : MonoBehaviour
    {
        [field: SerializeField] public DailyCalendarConfig DailyCalendarConfig { get; private set; }
    }
}