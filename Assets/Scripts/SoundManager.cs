using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get; private set;
    }

    [SerializeField] private SoundClipRefsSO soundClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
    }

    public void PlayCountdownSound(Vector3 position, float volume)
    {
        PlaySound(soundClipRefsSO.countdown, position, volume);
    }
    public void PlayGetFishSound(Vector3 position, float volume)
    {
        PlaySound(soundClipRefsSO.getFish, position, volume);
    }
    public void PlayHurtSound(Vector3 position, float volume)
    {
        PlaySound(soundClipRefsSO.hurt, position, volume);
    }
}
