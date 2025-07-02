using System;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UISample.UI;

namespace UISample.Features
{
    public class Shop : ILocalService, IInitializable
    {
        private const string AdvMessage = "Would you like to see an advertisement?";
        private readonly ShopConfig _config;
        private readonly PlayerData _playerData;
        private readonly IAdvManager _advManager;
        private readonly IPurchaseManager _purchaseManager;
        private ShopController _shopController;
        private AcceptPopup _acceptPopup;
        private RouletteController _rouletteController;
        private string _currentProductId;
        public bool IsInitialized { get; private set; }
        public event Action<bool> OnPurchaseCompleted;
        
        public Shop(MainMenuConfigs configsContainer)
        {
            _config = configsContainer.ShopConfig;
            _playerData = ServiceLocator.Get<PlayerData>();
            _advManager = ServiceLocator.Get<IAdvManager>();
            _purchaseManager = ServiceLocator.Get<IPurchaseManager>();
        }

        public void Initialize()
        {
            var sceneUI = ServiceLocator.Get<SceneUI>();
            _shopController = sceneUI.GetController<ShopController>();
            _acceptPopup = sceneUI.GetController<AcceptPopup>();
            _rouletteController = sceneUI.GetController<RouletteController>();
            _shopController.InitializeSlots();
            IsInitialized = true;
        }

        public void TryPurchaseProduct(int index)
        {
            var product = _config.GetProduct(index);
            _currentProductId = product.Id;
            var cost = product.Cost;
            bool success = false;
            switch (product.Currency)
            {
                case ECurrency.Acorns:
                    success = TrySpendValue(_playerData.Acorns, cost);
                    break;
                case ECurrency.Gems:
                    success = TrySpendValue(_playerData.Gems, cost);
                    break;
                case ECurrency.RealMoney:
                    _purchaseManager.OnProductPurchased += HandlePurchaseResult;
                    _purchaseManager.Purchase(product.Id);
                    return;
                case ECurrency.Adv:
                    _acceptPopup.Show(AdvMessage);
                    _acceptPopup.OnClose += AdvChoiceResult;
                    return;
            }
            PurchaseCompleted(product.Id, success);
        }

        public void ConsumeProduct(string productId)
        {
            var product = _config.GetProduct(productId);
            switch (product.Type)
            {
                case EProducts.Gem:
                    _playerData.Gems.Value += product.Amount;
                    break;
                case EProducts.RandomSkin:
                    // Handle unlock random skin
                    //_rouletteController.Show();
                    break;
            }
        }

        private void PurchaseCompleted(string productId, bool success)
        {
            if (success)
                ConsumeProduct(productId);
            OnPurchaseCompleted?.Invoke(success);
            _currentProductId = null;
        }

        private bool TrySpendValue(PlayerDataValue<int> data, int amount)
        {
            if (data.Value < amount)
                return false;
            data.Value -= amount;
            return true;
        }

        private void HandlePurchaseResult(string productId, bool success)
        {
            _purchaseManager.OnProductPurchased -= HandlePurchaseResult;
            PurchaseCompleted(productId, success);
        }

        private void AdvChoiceResult(bool showRewardAdv)
        {
            _acceptPopup.OnClose -= AdvChoiceResult;
            if (showRewardAdv)
            {
                _advManager.OnRewardAdvShown += HandleRewardAdvResult;
                _advManager.ShowRewardAdv();
            }
            else
            {
                PurchaseCompleted(_currentProductId, false);
            }
        }

        private void HandleRewardAdvResult(bool success)
        {
            _advManager.OnRewardAdvShown -= HandleRewardAdvResult;
            PurchaseCompleted(_currentProductId, success);
        }
    }
}