using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class MainMenuView : BaseView
    {
        [field: SerializeField] public Button PlayButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: SerializeField] public TMP_Text AcornsText { get; private set; }
        [field: SerializeField] public TMP_Text GemsText { get; private set; }
        [field: SerializeField] public Button DailyQuestsButton { get; private set; }
        [field: SerializeField] public GameObject DailyQuestsNotification { get; private set; }
        [field: SerializeField] public Button DailyCalendarButton { get; private set; }
        [field: SerializeField] public GameObject DailyCalendarNotification { get; private set; }
        [field: SerializeField] public TMP_Text DailyCalendarTimerText { get; private set; }
    }
}