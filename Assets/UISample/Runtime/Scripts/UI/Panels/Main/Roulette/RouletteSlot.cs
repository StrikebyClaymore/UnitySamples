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
        public int Index { get; private set; }

        public void SetData(int index, Sprite icon)
        {
            Index = index;
            _icon.sprite = icon;
        }

        public void SetDefault()
        {
            _background.Hide();
        }

        public void SetWin()
        {
            _background.Show();
        }
    }
}