using System;
using TMPro;
using UISample.Features;
using UISample.UI.UIElements;
using UISample.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class RewardSlot : ClickableLayoutSlot
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private GameObject _frame;
        [SerializeField] private GameObject _rewarded;

        public void Initialize(DailyReward item, DailyRewardData data, int index, Action<int> clickAction)
        {
            base.Initialize(index, clickAction);
            _icon.sprite = data.Icon;
            _amountText.text = item.Amount.ToString();
            SetState(item.State);
        }
        
        public void SetState(EDailyRewardState newState)
        {
            switch (newState)
            {
                case EDailyRewardState.Locked:
                    Lock();
                    break;
                case EDailyRewardState.Unlocked:
                    Unlock();
                    break;
                case EDailyRewardState.Rewarded:
                    Reward();
                    break;
            }
        }

        private void Lock()
        {
            _button.interactable = false;
            _icon.gameObject.Show();
            _amountText.gameObject.Show();
            _frame.Hide();
            _rewarded.Hide();
        }

        private void Unlock()
        {
            _button.interactable = true;
            _icon.gameObject.Show();
            _amountText.gameObject.Show();
            _frame.Show();
            _rewarded.Hide();
        }

        private void Reward()
        {
            _button.interactable = false;
            _icon.gameObject.Hide();
            _amountText.gameObject.Hide();
            _frame.Hide();
            _rewarded.Show();
        }
    }
}