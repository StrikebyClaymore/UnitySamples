using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class AcceptPopupView : PopupView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public Button AcceptButton { get; private set; }
        [field: SerializeField] public Button CancelButton { get; private set; }
        [field: SerializeField] public TMP_Text MessageText { get; private set; }
    }
}