using UISample.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class RouletteSlot : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Rect { get; private set; }
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
            _background.Hide();
        }

        public void SetWin()
        {
            _background.Show();
        }
    }
}