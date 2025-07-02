using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class RouletteView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public RouletteSlotsContainer[] SlotContainers { get; private set; }
        [field: SerializeField] public CanvasGroup StartPanel { get; private set; }
        [field: SerializeField] public Button StartButton { get; private set; }
        [field: SerializeField] public RectTransform CenterPoint { get; private set; }
    }
}