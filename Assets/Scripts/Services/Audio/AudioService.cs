using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.Audio
{
    /// <summary>
    /// Audio service implementation for managing music and sound effects
    /// </summary>
    public class AudioService : IAudioService
    {
        private AudioSource _musicSource;
        private List<AudioSource> _sfxSources = new List<AudioSource>();
        private GameObject _audioRoot;

        private float _masterVolume = 1.0f;
        private float _musicVolume = 1.0f;
        private float _sfxVolume = 1.0f;

        public async Task InitializeAsync()
        {
            // Create audio root object
            _audioRoot = new GameObject("AudioService");
            Object.DontDestroyOnLoad(_audioRoot);

            // Create music audio source
            _musicSource = _audioRoot.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;

            Debug.Log("[AudioService] Initialized successfully.");
            await Task.CompletedTask;
        }

        public async Task PlayMusicAsync(AudioClip clip, bool loop = true)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioService] Cannot play null music clip.");
                return;
            }

            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.volume = _masterVolume * _musicVolume;
            _musicSource.Play();

            Debug.Log($"[AudioService] Playing music: {clip.name}");
            await Task.CompletedTask;
        }

        public async Task PlaySFXAsync(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioService] Cannot play null SFX clip.");
                return;
            }

            // Find or create available SFX source
            AudioSource sfxSource = GetAvailableSFXSource();
            sfxSource.clip = clip;
            sfxSource.volume = _masterVolume * _sfxVolume * volume;
            sfxSource.loop = false;
            sfxSource.Play();

            await Task.CompletedTask;
        }

        public void StopMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
                Debug.Log("[AudioService] Music stopped.");
            }
        }

        public void StopAllSFX()
        {
            foreach (var sfxSource in _sfxSources)
            {
                sfxSource.Stop();
            }
            Debug.Log("[AudioService] All SFX stopped.");
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        public float GetMasterVolume() => _masterVolume;
        public float GetMusicVolume() => _musicVolume;
        public float GetSFXVolume() => _sfxVolume;

        public bool IsMusicPlaying()
        {
            return _musicSource != null && _musicSource.isPlaying;
        }

        private AudioSource GetAvailableSFXSource()
        {
            // Find existing non-playing source
            foreach (var source in _sfxSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // Create new source if none available
            var newSource = _audioRoot.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = false;
            _sfxSources.Add(newSource);

            return newSource;
        }

        private void UpdateVolumes()
        {
            UpdateMusicVolume();
            // SFX volume is applied when playing individual clips
        }

        private void UpdateMusicVolume()
        {
            if (_musicSource != null)
            {
                _musicSource.volume = _masterVolume * _musicVolume;
            }
        }
    }
}
