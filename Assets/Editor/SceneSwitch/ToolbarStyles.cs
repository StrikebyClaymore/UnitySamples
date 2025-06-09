using UnityEngine;

namespace UnitySamples.Editor
{
    public static class ToolbarStyles
    {
        public static readonly GUIStyle ButtonStyle;

        static ToolbarStyles()
        {
            ButtonStyle = new("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 160,
            };
        }
    }
}