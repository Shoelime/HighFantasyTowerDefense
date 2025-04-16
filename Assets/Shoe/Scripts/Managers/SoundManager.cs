using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ISoundManager
{
    private int maxSimultaneousImpactSounds = 3;
    private int maxSimultaneousDeathSounds = 3;
    private int maxSimultaneousLaunchSounds = 3;
    private int maxSimultaneousOtherSounds = 10;

    private Dictionary<SoundType, Queue<SoundData>> activeSounds;
    private CoroutineMonoBehavior audioPlayerCoroutine;
    private GameObject audioPlayerCoroutineObject;

    public void Initialize()
    {
        activeSounds = new Dictionary<SoundType, Queue<SoundData>>();

        audioPlayerCoroutineObject = new GameObject("Audio Player Object");
        audioPlayerCoroutine = audioPlayerCoroutineObject.AddComponent<CoroutineMonoBehavior>();
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

        if (!activeSounds.ContainsKey(data.soundType))
        {
            activeSounds[data.soundType] = new Queue<SoundData>();
        }

        Queue<SoundData> soundQueue = activeSounds[data.soundType];

        if (soundQueue.Count >= GetMaxSimultaneousSounds(data.soundType))
        {
            // Remove oldest sound only if it's still in the queue
            if (soundQueue.TryDequeue(out SoundData oldestSound))
            {
                audioSource.Stop();
            }
        }

        audioPlayerCoroutine.StartCoroutine(PlaySoundCoroutine(audioSource, data, volume));
        soundQueue.Enqueue(data);
    }

    /// <summary>
    /// Coroutine for playing the sound
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    /// <param name="soundType"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    private IEnumerator PlaySoundCoroutine(AudioSource audioSource, SoundData data, float volume = 1f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(data.GetRandomClip());

        yield return new WaitForSeconds(data.clipDuration);

        if (activeSounds.TryGetValue(data.soundType, out Queue<SoundData> soundQueue) && soundQueue.Count > 0)
        {
            soundQueue.Dequeue();
        }
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
