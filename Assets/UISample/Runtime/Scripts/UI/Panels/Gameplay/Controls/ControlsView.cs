using UISample.UI.UIElements;
using UnityEngine;

namespace UISample.UI
{
    public class ControlsView : BaseView
    {
        [field: SerializeField] public Button TopButton { get; private set; }
        [field: SerializeField] public Button RightButton { get; private set; }
        [field: SerializeField] public Button DownButton { get; private set; }
        [field: SerializeField] public Button LeftButton { get; private set; }
    }
}