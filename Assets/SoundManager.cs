using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int maxAudioSources = 10; // Số lượng AudioSource tối đa

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip _sound, float volume = 1.0f)
    {
        if (audioSources.Count >= maxAudioSources)
            return;

        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = _sound;
        tempSource.volume = volume;
        tempSource.Play();
        audioSources.Add(tempSource);

        StartCoroutine(RemoveAudioSource(tempSource));
    }

    private IEnumerator RemoveAudioSource(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        audioSources.Remove(source);
        Destroy(source);
    }
}
