using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class HUDView : BaseView
    {
        [field: SerializeField] public TMP_Text AcornsText { get; private set; }
        [field: SerializeField] public Button PauseButton { get; private set; }
    }
}