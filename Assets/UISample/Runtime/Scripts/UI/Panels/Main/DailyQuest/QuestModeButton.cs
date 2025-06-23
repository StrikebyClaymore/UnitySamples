using UISample.UI.UIElements;
using UnityEngine;

namespace UISample.UI
{
    public class QuestModeButton : Button
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hightlightColor;

        public void EnableHighlight()
        {
            image.color = _hightlightColor;
        }

        public void DisableHighlight()
        {
            image.color = _defaultColor;            
        }
    }
}