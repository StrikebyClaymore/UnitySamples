using UISample.UI.UIElements;
using UnityEngine;

namespace UISample.UI
{
    public class ControlsView : BaseView
    {
        [field: SerializeField] public Button TopButton { get; set; }
        [field: SerializeField] public Button RightButton { get; set; }
        [field: SerializeField] public Button DownButton { get; set; }
        [field: SerializeField] public Button LeftButton { get; set; }
    }
}