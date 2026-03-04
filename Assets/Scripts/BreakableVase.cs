using UnityEngine;
// This script is attatched to the non-broken version of the vase
public class BreakableVase : MonoBehaviour{
    public GameObject brokenVase;  // Broken vase prefab
    public float breakForce = 5f;  // unit of force to break vase
    // 1 = easily broken   20 = basically indestructible
    public float timeRespawn = 5f; // Amount of time before respawning a new vase

    public RegenerateVase spawner; // to create a new vase

    private void OnCollisionEnter(Collision collision) {
        // If the velocity acting on the vase is greater than breakForce
        if (collision.relativeVelocity.magnitude > breakForce) {
            Break();  // "break" vase
        }
    }

    void Break() {
        // create the broken vase
        GameObject shards = Instantiate(brokenVase, transform.position, transform.rotation);

        // Check if a spawner exists
        // This should be attatched to an empty obj in the scene
        if (spawner != null) {
            spawner.OnVaseBroken();
        }
        
        // destroy non-broken version of the vase
        Destroy(gameObject);

        // remove vase shards after a specific amount of time
        Destroy(shards, timeRespawn);
    }
}
