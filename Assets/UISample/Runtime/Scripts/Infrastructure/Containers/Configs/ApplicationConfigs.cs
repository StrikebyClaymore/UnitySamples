using UISample.Data;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class ApplicationConfigs : MonoBehaviour
    {
        [field: SerializeField] public AudioConfig AudioConfig { get; private set; }
        [field: SerializeField] public DailyQuestsConfig DailyQuestsConfig { get; private set; }
    }
}