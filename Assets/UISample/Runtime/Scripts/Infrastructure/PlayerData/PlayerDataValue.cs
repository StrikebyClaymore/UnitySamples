using System;

namespace UISample.Infrastructure
{
    public class PlayerDataValue<T>
    {
        private readonly PlayerPrefsData _data;
        private readonly string _saveKey;
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if(!_initialized)
                    return;
                _data.SetValue<T>(_saveKey, _value);
                OnValueChanged?.Invoke(_value);
            }
        }
        private bool _initialized;
        public event Action<T> OnValueChanged;

        public PlayerDataValue(PlayerPrefsData data, string saveKey, T defaultValue = default,
            bool resetOnCreate = false)
        {
            _data = data;
            _saveKey = saveKey;
            if (resetOnCreate)
            {
                _data.SetValue<T>(_saveKey, defaultValue);
                Value = defaultValue;
            }
            else
            {
                Value = _data.GetValue<T>(_saveKey, defaultValue);
            }
        }

        public void Initialize()
        {
            _initialized = true;
            OnValueChanged?.Invoke(_value);
        }
    }
}