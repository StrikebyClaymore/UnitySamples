using System;
using UnityEngine;

namespace UISample.UI.UIElements
{
    public abstract class ClickableLayoutSlot : MonoBehaviour, ILayoutSlot
    {
        [SerializeField] protected UnityEngine.UI.Button _button;
        public int Index { get; protected set; }
        private Action<int> _clickAction; 

        protected virtual void Awake()
        {
            _button.onClick.AddListener(ButtonPressed);
        }

        protected virtual void Initialize(int index, Action<int> clickAction)
        {
            Index = index;
            _clickAction = clickAction;
        }

        protected virtual void ButtonPressed() => _clickAction?.Invoke(Index);
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (_button == null)
                _button = GetComponent<UnityEngine.UI.Button>();
        }
#endif
    }
}