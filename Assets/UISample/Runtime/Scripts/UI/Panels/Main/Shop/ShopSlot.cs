using System;
using TMPro;
using UISample.Features;
using UISample.UI.UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class ShopSlot : ClickableLayoutSlot
    {
        [SerializeField] private Image _productIcon;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Image _costIcon;
        [SerializeField] private TMP_Text _costText;
        
        public void Initialize(ProductData data, Sprite icon, Sprite costIcon, int index, Action<int> clickAction)
        {
            base.Initialize(index, clickAction);
            _productIcon.sprite = icon;
            _costIcon.sprite = costIcon;
            if (_amountText != null)
                _amountText.text = data.Amount.ToString();
            if(data.Currency != ECurrency.Adv)
                _costText.text = data.Cost.ToString();
        }
    }
}