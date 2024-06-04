using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Shoe/SoundData")]
public class SoundData : ScriptableObject
{
    public SoundType soundType;
    public List<AudioClip> audioClips;

    public AudioClip GetRandomClip()
    {
        if (audioClips == null || audioClips.Count == 0)
            return null;

        int index = Random.Range(0, audioClips.Count);
        return audioClips[index];
    }
}

public enum SoundType
{
    Impact,
    Death,
    Launch,
    Other
}