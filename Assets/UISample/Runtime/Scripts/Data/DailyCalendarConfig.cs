using System.Collections.Generic;
using UISample.Features;
using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "DailyCalendarConfig", menuName = "UISample/Daily Calendar Config")]
    public class DailyCalendarConfig : ScriptableObject
    {
        [field: SerializeField] public float TimerInterval { get; private set; } = 60;
        [field: SerializeField] public int SlotsCount { get; private set; } = 28;
        [field: SerializeField] public List<DailyRewardData> Data { get; private set; }
        
        public DailyRewardData GetData(int id)
        {
            foreach (var data in Data)
            {
                if (data.Id == id)
                    return data;
            }

            return null;
        }
    }
}