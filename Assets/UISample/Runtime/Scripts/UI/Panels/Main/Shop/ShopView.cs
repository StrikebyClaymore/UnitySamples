using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class ShopView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        [field: SerializeField] public ShopSlot[] Slots { get; private set; }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
        }
    }
}