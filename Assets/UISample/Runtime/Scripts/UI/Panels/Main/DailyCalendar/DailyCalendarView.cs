using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class DailyCalendarView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public TMP_Text TimerText { get; private set; }
        [field: SerializeField] public RewardSlot[] Slots { get; private set; }
    }
}