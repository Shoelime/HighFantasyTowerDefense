using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public int maxSimultaneousImpactSounds = 3;
    public int maxSimultaneousDeathSounds = 3;
    public int maxSimultaneousLaunchSounds = 3;
    public int maxSimultaneousOtherSounds = 10;

    private Dictionary<SoundType, int> activeSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);      
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeSoundLimits();
    }

    /// <summary>
    /// Set the starting values
    /// </summary>
    private void InitializeSoundLimits()
    {
        activeSounds = new Dictionary<SoundType, int>();

        activeSounds[SoundType.Impact] = 0;
        activeSounds[SoundType.Death] = 0;
        activeSounds[SoundType.Launch] = 0;
        activeSounds[SoundType.Other] = 0;
    }

    /// <summary>
    /// Call to play sound unless its there are too many of the type playing already
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="data"></param>
    /// <param name="volume"></param>
    public void PlaySound(AudioSource audioSource, SoundData data, float volume = 1.0f)
    {
        int maxSimultaneousSounds = GetMaxSimultaneousSounds(data.soundType);

        if (activeSounds[data.soundType] < maxSimultaneousSounds)
        {
            if (audioSource != null && data != null)
            {
                StartCoroutine(PlaySoundCoroutine(audioSource, data.GetRandomClip(), data.soundType, volume));
            }
            else Debug.LogError("Can't play sound because of lack of components");
        }
    }

    /// <summary>
    /// Coroutine for playing the sound
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    /// <param name="soundType"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    private IEnumerator PlaySoundCoroutine(AudioSource audioSource, AudioClip clip, SoundType soundType, float volume)
    {
        activeSounds[soundType]++;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);

        yield return new WaitForSeconds(clip.length);

        activeSounds[soundType]--;
    }

    /// <summary>
    /// Get the amount of sounds currently playing of the type so we don't overlap too much
    /// </summary>
    /// <param name="soundType"></param>
    /// <returns></returns>
    private int GetMaxSimultaneousSounds(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.Impact => maxSimultaneousImpactSounds,
            SoundType.Death => maxSimultaneousDeathSounds,
            SoundType.Launch => maxSimultaneousLaunchSounds,
            SoundType.Other => maxSimultaneousOtherSounds,
            _ => 1,
        };
    }
}
