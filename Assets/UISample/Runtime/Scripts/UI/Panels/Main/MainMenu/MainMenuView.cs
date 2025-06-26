using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class MainMenuView : BaseView
    {
        [field: Header("Top Panel")]
        [field: SerializeField] public TMP_Text AcornsText { get; private set; }
        [field: SerializeField] public TMP_Text GemsText { get; private set; }
        [field: Header("Daily-Weekly")]
        [field: SerializeField] public Button DailyQuestsButton { get; private set; }
        [field: SerializeField] public GameObject DailyQuestsNotification { get; private set; }
        [field: SerializeField] public Button DailyCalendarButton { get; private set; }
        [field: SerializeField] public GameObject DailyCalendarNotification { get; private set; }
        [field: SerializeField] public TMP_Text DailyCalendarTimerText { get; private set; }
        [field: Header("Character")]
        [field: SerializeField] public MenuCharacter MenuCharacter { get; private set; }
        [field: Header("Play")]
        [field: SerializeField] public Button PlayButton { get; private set; }
        [field: Header("Navigation")]
        [field: SerializeField] public Button PersonalButton { get; private set; }
        [field: SerializeField] public Button SkinsButton { get; private set; }
        [field: SerializeField] public Button ShopButton { get; private set; }
        [field: SerializeField] public Button LeaderboardButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
    }
}