using System;

namespace UISample.Utilities
{
    public class ReactiveProperty<T> : IDisposable where T : struct
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
        public event Action<T> OnValueChanged;

        public ReactiveProperty(T defaultValue = default)
        {
            Value = defaultValue;
        }

        public void Dispose()
        {
            OnValueChanged = null;
        }
    }
}