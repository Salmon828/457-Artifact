using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    public PlayableDirector timeline;

    private bool _triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (_triggered || !other.CompareTag("Player")) return;

        _triggered = true;
        timeline.Play();

    }
}
