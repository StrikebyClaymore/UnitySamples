using System;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class PlayerPrefsData
    {
        public T GetValue<T>(string key, T defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.Log($"Key '{key}' not found. Returning default value.");
                return defaultValue;
            }
            if (typeof(T) == typeof(int))
            {
                var intValue = GetInt(key, Convert.ToInt32(defaultValue));
                return (T)(object)intValue;
            }
            else if (typeof(T) == typeof(float))
            {
                var floatValue = GetFloat(key, Convert.ToSingle(defaultValue));
                return (T)(object)floatValue;
            }
            else if (typeof(T) == typeof(string))
            {
                var stringValue = GetString(key, Convert.ToString(defaultValue));
                return (T)(object)stringValue;
            }
            else if (typeof(T) == typeof(bool))
            {
                var boolValue = GetBool(key, Convert.ToBoolean(defaultValue));
                return (T)(object)boolValue;
            }
            else
            {
                throw new ArgumentException($"Unsupported type {typeof(T)} for key '{key}'. Returning default value: {defaultValue}");
            }
        }
        
        public void SetValue<T>(string key, T value)
        {
            if (value == null)  
            {
                Debug.LogError($"Value for key '{key}' is null.");
                return;
            }
            object objValue = value;
            switch (objValue)
            {
                case int intValue:
                    PlayerPrefs.SetInt(key, intValue);
                    break;
                case float floatValue:
                    PlayerPrefs.SetFloat(key, floatValue);
                    break;
                case string stringValue:
                    PlayerPrefs.SetString(key, stringValue);
                    break;
                case bool boolValue:
                    PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
                    break;
                default:
                    Debug.LogWarning($"Value '{value}' has unsupported type {typeof(T)}.");
                    break;
            }
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetFloat(string key, float value)
        { 
            PlayerPrefs.SetFloat(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
        
        public T GetData<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key) || !PlayerPrefs.HasKey(key))
                return null;
            var value = PlayerPrefs.GetString(key, string.Empty);
            if (!string.IsNullOrEmpty(value))
                return JsonUtility.FromJson<T>(value);
            return null;
        }

        public void SetData<T>(string key, T value) where T : class
        {
            if (value == null || string.IsNullOrEmpty(key))
                return;
            var json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}