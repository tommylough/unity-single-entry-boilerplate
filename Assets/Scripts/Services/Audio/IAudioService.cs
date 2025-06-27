using System.Threading.Tasks;
using UnityEngine;

namespace Services.Audio
{
    /// <summary>
    /// Interface for audio management service
    /// </summary>
    public interface IAudioService
    {
        /// <summary>
        /// Play background music
        /// </summary>
        Task PlayMusicAsync(AudioClip clip, bool loop = true);

        /// <summary>
        /// Play a sound effect
        /// </summary>
        Task PlaySFXAsync(AudioClip clip, float volume = 1.0f);

        /// <summary>
        /// Stop background music
        /// </summary>
        void StopMusic();

        /// <summary>
        /// Stop all sound effects
        /// </summary>
        void StopAllSFX();

        /// <summary>
        /// Set master volume (0.0 to 1.0)
        /// </summary>
        void SetMasterVolume(float volume);

        /// <summary>
        /// Set music volume (0.0 to 1.0)
        /// </summary>
        void SetMusicVolume(float volume);

        /// <summary>
        /// Set SFX volume (0.0 to 1.0)
        /// </summary>
        void SetSFXVolume(float volume);

        /// <summary>
        /// Get current master volume
        /// </summary>
        float GetMasterVolume();

        /// <summary>
        /// Get current music volume
        /// </summary>
        float GetMusicVolume();

        /// <summary>
        /// Get current SFX volume
        /// </summary>
        float GetSFXVolume();

        /// <summary>
        /// Check if music is currently playing
        /// </summary>
        bool IsMusicPlaying();
    }
}
