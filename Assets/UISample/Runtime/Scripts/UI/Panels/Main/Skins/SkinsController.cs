using System.Collections.Generic;
using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.UI
{
    public class SkinsController : BaseController
    {
        private readonly SkinsView _view;
        private readonly SkinsConfig _config;
        private readonly PlayerData _playerData;
        private readonly List<SkinSlot> _slots = new();
        private Skin[] _skins;
        private int _selectedSlotIndex = -1;

        public SkinsController(UIContainer uiContainer, MainMenuConfigs configsContainer)
        {
            _view = uiContainer.GetView<SkinsView>();
            _config = configsContainer.SkinsConfig;
            _playerData = ServiceLocator.Get<PlayerData>();
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
        
        public void InitializeSlots(Skin[] skins)
        {
            _skins = skins;
            for (int i = 0; i < _config.Skins.Count; i++)
            {
                var skinData = _config.Skins[i];
                var slot = GameObject.Instantiate(_config.SkinSlotPrefab, _view.SlotsContainer);
                slot.Initialize(skinData, _skins[i].IsUnlocked, i, SkinSlotPressed);
                _slots.Add(slot);
            }
            SkinSlotPressed(_playerData.SelectedSkin.Value);
        }
        
        private void ClosePressed()
        {
            _sceneUI.HideController<SkinsController>();
        }

        private void SkinSlotPressed(int index) 
        {
            var skin = _skins[index];
            if(index == _selectedSlotIndex || !skin.IsUnlocked)
                return;
            if (_selectedSlotIndex != -1)
                _slots[_selectedSlotIndex].SetState(ESkinState.Unlocked);
            _selectedSlotIndex = index;
            _playerData.SelectedSkin.Value = _selectedSlotIndex;
            _slots[_selectedSlotIndex].SetState(ESkinState.Selected);
            _view.MenuCharacter.SetHatSprite(index == 0 ? null :  _config.Skins[_selectedSlotIndex].Sprite);
        }
    }
}