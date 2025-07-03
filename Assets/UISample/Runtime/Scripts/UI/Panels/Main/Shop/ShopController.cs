using System.Collections.Generic;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;
using UISample.Utility;

namespace UISample.UI
{
    public class ShopController : BaseController
    {
        private readonly ShopView _view;
        private readonly ShopConfig _config;
        private readonly ShopManager _shopManager;
        private readonly List<ShopSlot> _slots = new();

        public ShopController(UIContainer uiContainer, MainMenuConfigs configsContainer)
        {
            _view = uiContainer.GetView<ShopView>();
            _config = configsContainer.ShopConfig;
            _shopManager = ServiceLocator.GetLocal<ShopManager>();
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

        public void HideSkinSlots()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_config.Products[i].Type is EProducts.RandomSkin)
                    _slots[i].Hide();
            }
        }

        private void SlotPressed(int index)
        {
            _shopManager.TryPurchaseProduct(index);
        }

        private void ClosePressed()
        {
            _sceneUI.HideController<ShopController>();
        }
    }
}