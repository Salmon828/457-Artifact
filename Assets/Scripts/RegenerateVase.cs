using UnityEngine;

// Logic to make vase respawn after being broken
// Attatch to an empty game object located in the spot vase should always respawn to
public class RegenerateVase : MonoBehaviour {
    private GameObject currentVase;
    public GameObject vase;
    public float respawnDelay = 2f;

    // Spawns a vase right when the scene is run/played
    void Start(){
        SpawnVase();
    }

    public void SpawnVase() {
        currentVase = Instantiate(vase, transform.position, transform.rotation);
        currentVase.GetComponent<BreakableVase>().spawner = this;
    }

    public void OnVaseBroken() {
        Invoke(nameof(SpawnVase), respawnDelay);
    }
}
