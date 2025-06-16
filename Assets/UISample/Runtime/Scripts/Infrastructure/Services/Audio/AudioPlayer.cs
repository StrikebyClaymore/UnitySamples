using System.Collections.Generic;
using Plugins.ServiceLocator;
using Pool;
using UISample.Data;
using UnityEngine;
using UnityEngine.Events;

namespace UISample.Infrastructure
{
    public class AudioPlayer : IService, IUpdate
    {
        private readonly AudioConfig _config;
        public AudioConfig Config => _config;
        private readonly MonoPool<AudioSource> _audioPool;
        private readonly List<AudioSource> _activeSources = new();
        private readonly AudioSettings _audioSettings;
        public readonly UnityEvent<AudioSource> OnAudioPlayingCompleted = new();
        private AudioSource _backgroundMusic;
        
        public AudioPlayer(ConfigsContainer configsContainer)
        {
            _config = configsContainer.AudioConfig;
            _audioPool = new MonoPool<AudioSource>(_config.AudioSourcePrefab, 1, new GameObject("Audio Pool").transform, true);
            _audioSettings = ServiceLocator.Get<AudioSettings>();
            _audioSettings.OnMusicVolumeChanged.AddListener(OnMusicVolumeChanged);
            var applicationLoop = ServiceLocator.Get<ApplicationLoop>();
            applicationLoop.AddUpdatable(this);
        }
        
        public void CustomUpdate()
        {
            for (int i = _activeSources.Count - 1; i >= 0; i--)
            {
                var source = _activeSources[i];
                if (!source.isPlaying)
                    Stop(source);
            }
        }
        
        public AudioSource PlaySound(AudioClip clip, bool loop = false, float volume = 1.0f)
        {
            var source = _audioPool.Get();
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            source.Play();
            if(!loop)
                _activeSources.Add(source);
            return source;
        }
        
        public void PlayMusic(AudioClip clip)
        {
            if(_backgroundMusic && _backgroundMusic.clip == clip)
                return;
            _backgroundMusic = PlaySound(clip, true, _audioSettings.MusicVolume);
        }
        
        public AudioSource PlayUI(AudioClip clip)
        {
            return PlaySound(clip, false, _audioSettings.UIVolume);
        }
        
        public void Stop(AudioSource source)
        {
            source.Stop();
            _activeSources.Remove(source);
            _audioPool.Release(source);
            OnAudioPlayingCompleted?.Invoke(source);
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            if(_backgroundMusic)
                _backgroundMusic.volume = value;
        }
    }
}