using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "DailyCalendarConfig", menuName = "UISample/Daily Calendar Config")]
    public class DailyCalendarConfig : ScriptableObject
    {
        [field: SerializeField] public string TimerKey { get; private set; } = "DailyCalendarTimer";
        [field: SerializeField] public float TimerInterval { get; private set; } = 60;
    }
}