using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    // Used to manage what background music is playing (calm vs boss music)
    public AudioSource source;
    public AudioClip calmMusic;
    public AudioClip bossMusic;

    void Start()
    {
        PlayCalmMusic(); // Play calm music when scene starts
    }
    public void PlayBossMusic()
    {
        if(source.clip != bossMusic)
        {
            // if we're not already playing boss music
            source.clip = bossMusic;
            source.volume = 0.2f; // lower the volume
            source.Play();
        }
    }

    public void PlayCalmMusic()
    {
        if(source.clip != calmMusic)
        {
            // if we're not already playing calm music
            source.clip = calmMusic;
            source.volume = 1f;
            source.Play();
        }
        
    }
}
