using System;
using UISample.Features;
using UISample.UI.UIElements;
using UISample.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class SkinSlot : ClickableLayoutSlot
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _frame;
        [SerializeField] private Image _lock;
        
        public void Initialize(SkinData data, bool isUnlocked, int index, Action<int> clickAction)
        {
            base.Initialize(index, clickAction);
            _icon.sprite = data.Sprite;
            SetState(isUnlocked ? ESkinState.Unlocked : ESkinState.Locked);
        }

        public void SetState(ESkinState state)
        {
            switch (state)
            {
                case ESkinState.Locked:
                    Lock();
                    break;
                case ESkinState.Unlocked:
                    Unlock();
                    break;
                case ESkinState.Selected:
                    Select();
                    break;
            }
        }

        private void Lock()
        {
            _icon.Hide();
            _frame.Hide();
            _lock.Show();
        }

        private void Unlock()
        {
            _icon.Show();
            _frame.Hide();
            _lock.Hide();  
        }
        
        private void Select()
        {
            _icon.Show();
            _frame.Show();
            _lock.Hide();
        }
    }
}