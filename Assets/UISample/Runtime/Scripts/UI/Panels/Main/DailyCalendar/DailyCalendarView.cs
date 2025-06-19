using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class DailyCalendarView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShdowCloseButton { get; private set; }
    }
}