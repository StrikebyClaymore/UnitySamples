using System.Collections.Generic;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class ShopController : BaseController
    {
        private readonly ShopView _view;
        private readonly ShopConfig _config;
        private readonly Shop _shop;
        private readonly List<ShopSlot> _slots = new();

        public ShopController(UIContainer uiContainer, MainMenuConfigs configsContainer)
        {
            _view = uiContainer.GetView<ShopView>();
            _config = configsContainer.ShopConfig;
            _shop = ServiceLocator.GetLocal<Shop>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
        }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }

        public void InitializeSlots()
        {
            for (int i = 0; i < _view.Slots.Length; i++)
            {
                var slot = _view.Slots[i];
                var data = _config.Products[i];
                slot.Initialize(data, _config.GetIcon(data.Type), _config.GetIcon(data.Currency), i, SlotPressed);
                _slots.Add(slot);
            }
        }

        private void SlotPressed(int index)
        {
            _shop.TryPurchaseProduct(index);
        }

        private void ClosePressed()
        {
            _sceneUI.HideController<ShopController>();
        }
    }
}