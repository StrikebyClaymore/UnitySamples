using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class HollowView : BaseView
    {
        [field: SerializeField] public Button Stop { get; private set; }
        [field: SerializeField] public Button Continue { get; private set; }
    }
}