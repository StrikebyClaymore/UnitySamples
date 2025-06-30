using Plugins.ServiceLocator;
using UnityEngine;
using UnityEngine.Events;

namespace UISample.Infrastructure
{
    public class AudioSettings : IService, IInitializable
    {
        private readonly PlayerData _playerData;
        public float SoundVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float UIVolume { get; private set; }
        public bool IsInitialized { get; private set; }
        private float _lastSoundVolume;
        private float _lastMusicVolume;
        private float _lastUIVolume;
        public readonly UnityEvent<float> OnSoundVolumeChanged = new();
        public readonly UnityEvent<float> OnMusicVolumeChanged = new();
        public readonly UnityEvent<float> OnUIVolumeChanged = new();

        public AudioSettings()
        {
            _playerData = ServiceLocator.Get<PlayerData>();
        }
        
        public void Initialize()
        {
            SetSoundVolume(_playerData.SoundVolume.Value);
            SetMusicVolume(_playerData.MusicVolume.Value);
            SetUIVolume(_playerData.UIVolume.Value);
            IsInitialized = true;
        }
        
        public void SetSoundVolume(float value)
        {
            SoundVolume = value;
            _playerData.SoundVolume.Value = value;
            AudioListener.volume = SoundVolume;
            OnSoundVolumeChanged?.Invoke(SoundVolume);
        }

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            _playerData.MusicVolume.Value = value;
            OnMusicVolumeChanged?.Invoke(value);
        }
        
        public void SetUIVolume(float value)
        {
            UIVolume = value;
            _playerData.UIVolume.Value = value;
            OnUIVolumeChanged?.Invoke(value);
        }
        
        public void ToggleSound(bool enable)
        {
            if (enable)
            {
                SoundVolume = _lastSoundVolume;
            }
            else
            {
                _lastSoundVolume = SoundVolume;
                SoundVolume = 0;
            }
            _playerData.SoundVolume.Value = SoundVolume;
            AudioListener.volume = SoundVolume;
            OnSoundVolumeChanged?.Invoke(SoundVolume);
        }
        
        public void ToggleMusic(bool enable)
        {
            if (enable)
            {
                MusicVolume = _lastMusicVolume;
            }
            else
            {
                _lastMusicVolume = MusicVolume;
                MusicVolume = 0;
            }
            _playerData.MusicVolume.Value = MusicVolume;
            OnMusicVolumeChanged?.Invoke(MusicVolume);
        }
        
        public void ToggleUI(bool enable)
        {
            if (enable)
            {
                UIVolume = _lastUIVolume;
            }
            else
            {
                _lastUIVolume = UIVolume;
                UIVolume = 0;
            }
            _playerData.UIVolume.Value = UIVolume;
            OnUIVolumeChanged?.Invoke(UIVolume);
        }
    }
}