using System;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.Utility
{
    public class PersistentTimer : IUpdate
    {
        private readonly string _key;
        private readonly float _duration;
        private DateTime _savedTime;
        public event Action<TimeSpan> OnUpdate;
        public event Action OnComplete;

        public PersistentTimer(string key, float time)
        {
            _key = key;
            _duration = time;
            if (PlayerPrefs.HasKey(_key))
                _savedTime = DateTime.Parse(PlayerPrefs.GetString(_key));
        }

        public void CustomUpdate()
        {
            if (PlayerPrefs.HasKey(_key))
            {
                TimeSpan timePassed = DateTime.UtcNow - _savedTime;
                TimeSpan remaining = TimeSpan.FromSeconds(_duration) - timePassed;
                if (remaining.TotalSeconds < 0)
                    remaining = TimeSpan.Zero;
                OnUpdate?.Invoke(remaining);
            }
        }
        
        public void Start()
        {
            if (PlayerPrefs.HasKey(_key))
            {
                TimeSpan timePassed = DateTime.UtcNow - _savedTime;
                if (timePassed.TotalSeconds >= _duration)
                {
                    Debug.Log("Таймер завершился.");
                    PlayerPrefs.DeleteKey(_key);
                    PlayerPrefs.Save();
                    OnComplete?.Invoke();
                }
            }
            else
            {
                _savedTime = DateTime.UtcNow;
                PlayerPrefs.SetString(_key, _savedTime.ToString("o"));
                PlayerPrefs.Save();
                Debug.Log("Новый таймер запущен.");
            }
        }

        public void Stop()
        {
            PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
            Debug.Log("Таймер остановлен вручную.");
        }
    }
}