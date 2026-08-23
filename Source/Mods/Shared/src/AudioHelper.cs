using System;
using Il2CppScheduleOne.Money;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Centralized safe audio feedback helper for Schedule I mods.
/// Provides native cash sounds and procedural click/deny/chime tones.
/// </summary>
public static class AudioHelper
{
    private static AudioClip? _clickClip;
    private static AudioClip? _denyClip;
    private static AudioClip? _chimeClip;
    private static GameObject? _audioHost;
    private static AudioSource? _audioSource;

    private static void EnsureAudioSource()
    {
        if (_audioSource != null && _audioSource.Pointer != IntPtr.Zero && !_audioSource.WasCollected && _audioSource.gameObject != null && _audioSource.gameObject.Pointer != IntPtr.Zero) return;
        // Clean up stale host if source was destroyed
        if (_audioHost != null && (_audioHost.Pointer == IntPtr.Zero || _audioHost.WasCollected))
        {
            _audioHost = null;
            _audioSource = null;
        }

        try
        {
            _audioHost = new GameObject("S1Mods_AudioHost");
            UnityEngine.Object.DontDestroyOnLoad(_audioHost);
            _audioSource = _audioHost.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f; // 2D Sound
            _audioSource.volume = 0.50f;
        }
        catch
        {
            // Graceful fallback
        }
    }

    /// <summary>
    /// Plays the game's native cash sound (MoneyManager.Instance.PlayCashSound) safely.
    /// </summary>
    public static void PlayCashSound()
    {
        try
        {
            var money = MoneyManager.Instance;
            if (money != null && money.Pointer != IntPtr.Zero && !money.WasCollected)
            {
                money.PlayCashSound();
            }
        }
        catch
        {
            // Graceful fallback
        }
    }

    /// <summary>
    /// Plays a low-frequency alert buzz when an action or transaction is denied.
    /// </summary>
    public static void PlayDenySound(float volume = 0.45f)
    {
        try
        {
            EnsureAudioSource();
            if (_denyClip == null)
            {
                _denyClip = GenerateDenyTone();
            }
            if (_audioSource != null && _denyClip != null)
            {
                _audioSource.PlayOneShot(_denyClip, Mathf.Clamp01(volume));
            }
        }
        catch
        {
            // Graceful fallback
        }
    }

    /// <summary>
    /// Plays a subtle UI click when buttons or tabs are tapped.
    /// </summary>
    public static void PlayClickSound(float volume = 0.30f)
    {
        try
        {
            EnsureAudioSource();
            if (_clickClip == null)
            {
                _clickClip = GenerateClickTone();
            }
            if (_audioSource != null && _clickClip != null)
            {
                _audioSource.PlayOneShot(_clickClip, Mathf.Clamp01(volume));
            }
        }
        catch
        {
            // Graceful fallback
        }
    }

    /// <summary>
    /// Plays a pleasant chime tone (e.g. for level-ups, notifications, accomplishments).
    /// </summary>
    public static void PlayChimeSound(float volume = 0.35f)
    {
        try
        {
            EnsureAudioSource();
            if (_chimeClip == null)
            {
                _chimeClip = GenerateChimeTone();
            }
            if (_audioSource != null && _chimeClip != null)
            {
                _audioSource.PlayOneShot(_chimeClip, Mathf.Clamp01(volume));
            }
        }
        catch
        {
            // Graceful fallback
        }
    }

    private static AudioClip GenerateClickTone()
    {
        const int sampleRate = 44100;
        const float duration = 0.035f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float freq = Mathf.Lerp(1400f, 700f, (float)i / sampleCount);
            float envelope = 1f - ((float)i / sampleCount);
            samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.35f;
        }

        var clip = AudioClip.Create("S1Mods_Click", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip GenerateDenyTone()
    {
        const int sampleRate = 44100;
        const float duration = 0.15f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = Mathf.Clamp01((1f - ((float)i / sampleCount)) * 1.5f);
            float val = Mathf.Sin(2f * Mathf.PI * 180f * t) + 0.5f * Mathf.Sin(2f * Mathf.PI * 360f * t);
            samples[i] = Mathf.Clamp(val * 0.5f, -0.6f, 0.6f) * envelope * 0.45f;
        }

        var clip = AudioClip.Create("S1Mods_Deny", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip GenerateChimeTone()
    {
        const int sampleRate = 44100;
        const float duration = 0.25f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = Mathf.Exp(-5f * t);
            float val = Mathf.Sin(2f * Mathf.PI * 880f * t) + 0.5f * Mathf.Sin(2f * Mathf.PI * 1320f * t) + 0.25f * Mathf.Sin(2f * Mathf.PI * 1760f * t);
            samples[i] = val * envelope * 0.35f;
        }

        var clip = AudioClip.Create("S1Mods_Chime", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
