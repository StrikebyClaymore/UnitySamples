using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class SkinsView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public MenuCharacter MenuCharacter { get; private set; }
        [field: SerializeField] public Transform SlotsContainer { get; private set; }
    }
}