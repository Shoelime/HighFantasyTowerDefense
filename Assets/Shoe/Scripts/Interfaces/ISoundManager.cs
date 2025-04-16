using UnityEngine;

public interface ISoundManager : IService
{
    void PlaySound(AudioSource audioSource, SoundData data, float volume = 1.0f);
}