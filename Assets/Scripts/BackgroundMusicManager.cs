using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    // Used to manage what background music is playing (calm vs boss music)
    public AudioSource source;
    public AudioClip calmMusic;
    public AudioClip bossMusic;

    public float fadeDuration = 3f;

    void Start()
    {
        PlayCalmMusic(); // Play calm music when scene starts
    }

    public void PlayBossMusic()
    {
        if (source.clip != bossMusic)
        {
            source.clip = bossMusic;
            source.volume = 0.2f; // lower the volume
            source.Play();
        }
    }

    public void PlayCalmMusic()
    {
        if (source.clip != calmMusic)
        {
            source.clip = calmMusic;
            source.volume = 1f;
            source.Play();
        }
    }

    public void PlayBossMusicFade()
    {
        if (source.clip != bossMusic)
        {
            StopAllCoroutines();
            StartCoroutine(CrossfadeTo(bossMusic, 0.2f));
        }
    }

    public void PlayCalmMusicFade()
    {
        if (source.clip != calmMusic)
        {
            StopAllCoroutines();
            StartCoroutine(CrossfadeTo(calmMusic, 1f));
        }
    }

    private IEnumerator CrossfadeTo(AudioClip newClip, float targetVolume)
    {
        // Fade out
        float startVolume = source.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        source.volume = 0f;

        // Swap clip and fade in
        source.clip = newClip;
        source.Play();
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        source.volume = targetVolume;
    }
}
