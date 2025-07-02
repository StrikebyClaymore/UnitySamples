using UnityEngine;

namespace UISample.UI
{
    public class RouletteSlotsContainer : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [field: SerializeField] public RouletteSlot[] RouletteSlot { get; private set; }
    }
}