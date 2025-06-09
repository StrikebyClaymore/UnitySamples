using System.Collections.Generic;
using Plugins.ServiceLocator;
using Pool;
using UISample.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace UISample.Infrastructure
{
    public class AudioPlayer : IService, IInitializable, IUpdate
    {
        private readonly AudioConfig _config;
        private readonly MonoPool<AudioSource> _audioPool;
        private readonly List<AudioSource> _activeSources = new();
        private readonly AudioSettings _audioSettings;
        public readonly UnityEvent<AudioSource> OnAudioPlayingCompleted = new();
        private AudioSource _mainLoopMusic;
        
        public AudioPlayer(ConfigsContainer configsContainer)
        {
            _config = configsContainer.AudioConfig;
            _audioPool = new MonoPool<AudioSource>(_config.AudioSourcePrefab, 1, new GameObject("Audio Pool").transform);
            _audioSettings = ServiceLocator.Get<AudioSettings>();
            _audioSettings.OnMusicVolumeChanged.AddListener(OnMusicVolumeChanged);
            var applicationLoop = ServiceLocator.Get<ApplicationLoop>();
            applicationLoop.AddUpdatable(this);
        }

        public void Initialize()
        {
            _mainLoopMusic = PlaySound(_config.MainMusicClip, true, _audioSettings.MusicVolume);
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
        
        public AudioSource PlayMusic(AudioClip clip, bool loop = false)
        {
            return PlaySound(clip, loop, _audioSettings.MusicVolume);
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
            _mainLoopMusic.volume = value;
        }
    }
}