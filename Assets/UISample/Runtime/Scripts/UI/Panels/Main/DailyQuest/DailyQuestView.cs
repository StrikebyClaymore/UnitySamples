using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class DailyQuestView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
    }
}