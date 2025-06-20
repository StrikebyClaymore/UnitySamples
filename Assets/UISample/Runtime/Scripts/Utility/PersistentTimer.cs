using System;
using System.Globalization;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.Utility
{
    public class PersistentTimer : IUpdate
    {
        private readonly string _key;
        private readonly float _duration;
        private DateTime _savedTime;
        private bool _enabled;
        public event Action<TimeSpan> OnUpdate;
        public event Action OnComplete;

        public PersistentTimer(string key, float time)
        {
            _key = key;
            _duration = time;
            if (PlayerPrefs.HasKey(_key))
                _savedTime = DateTime.ParseExact(PlayerPrefs.GetString(_key), "o", CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal);
        }

        public void CustomUpdate()
        {
            if(!_enabled)
                return;
            if (PlayerPrefs.HasKey(_key))
            {
                TimeSpan timePassed = DateTime.UtcNow - _savedTime;
                TimeSpan remaining = TimeSpan.FromSeconds(_duration) - timePassed;
                if (remaining.TotalSeconds < 0)
                    remaining = TimeSpan.Zero;
                OnUpdate?.Invoke(remaining);
                if (timePassed.TotalSeconds >= _duration)
                {
                    Stop();
                    OnComplete?.Invoke();
                }
            }
        }
        
        public void Start(bool reset = false)
        {
            if(reset)
                DeleteEntry();
            if (PlayerPrefs.HasKey(_key))
            {
                TimeSpan timePassed = DateTime.UtcNow - _savedTime;
                if (timePassed.TotalSeconds >= _duration)
                {
                    Stop();
                    OnComplete?.Invoke();
                }
            }
            else
            {
                _savedTime = DateTime.UtcNow;
                PlayerPrefs.SetString(_key, _savedTime.ToString("o"));
                PlayerPrefs.Save();
                _enabled = true;
            }
        }

        public void Stop()
        {
            _enabled = false;
        }

        public void DeleteEntry()
        {
            PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
        }
    }
}