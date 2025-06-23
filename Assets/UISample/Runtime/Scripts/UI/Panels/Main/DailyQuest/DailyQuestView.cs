    using CustomInspector;
    using UISample.Features;
    using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class DailyQuestView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public ReorderableDictionary<EQuestCategory, QuestModeButton> Categories { get; private set; }
        [field: SerializeField] public Transform QuestContainer { get; private set; }
    }
}