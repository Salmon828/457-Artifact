using UnityEngine;
using System.Collections;

public class BossMusicStarts : MonoBehaviour
{
    // Added to StartFightTrigger object to play boss music for boss fight
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public BackgroundMusicManager music; // assign in Inspector
    public float delay = 5f; // wait time before starting
    private bool _triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if(_triggered || !other.CompareTag("Player"))return;
        _triggered = true;

        StartCoroutine(PlayBossMusicDelayed());
    }

    private IEnumerator PlayBossMusicDelayed()
    {
        yield return new WaitForSeconds(delay);
        if(music != null)
        {
            music.PlayBossMusic();
            Debug.Log("OMG ITS A BOSS FIGHT, BOSS MUSIC START");
        }
    }
}
