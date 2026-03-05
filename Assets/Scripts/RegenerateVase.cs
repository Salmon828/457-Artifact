using UnityEngine;

// Logic to make vase respawn after being broken
// Attatch to an empty game object located in the spot vase should always respawn to
public class RegenerateVase : MonoBehaviour {
    private GameObject currentVase;
    public GameObject vase;
    public float respawnDelay = 2f;

    // Spawns a vase right when the scene is run/played
    void Start(){
        if (vase == null) {
            Debug.LogWarning($"[{name}] no vase prefab assigned on {GetType().Name}. Assign a vase in the inspector.");
            return;
        }
        SpawnVase();
    }

    public void SpawnVase() {
        // Prevent spawning if there is already a vase scheduled or present
        if (currentVase != null) {
            Debug.LogWarning($"[{name}] SpawnVase called but currentVase already exists ('{currentVase.name}'). Skipping duplicate spawn.");
            return;
        }

        currentVase = Instantiate(vase, transform.position, transform.rotation);
        if (currentVase == null) {
            Debug.LogWarning($"[{name}] Instantiate returned null when spawning vase.");
            return;
        }
        var breakable = currentVase.GetComponent<BreakableVase>();
        if (breakable != null) {
            breakable.spawner = this;
        } else {
            Debug.LogWarning($"[{name}] spawned object '{currentVase.name}' does not have a BreakableVase component.\n" +
                "Assign the component to the vase prefab so it can notify the spawner when broken.");
        }
    }

    public void OnVaseBroken() {
        Invoke(nameof(SpawnVase), respawnDelay);
    }
}
