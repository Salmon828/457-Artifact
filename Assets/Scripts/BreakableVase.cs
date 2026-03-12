using UnityEngine;
// This script is attatched to the non-broken version of the vase
public class BreakableVase : MonoBehaviour{
    public GameObject brokenVase;  // Broken vase prefab
    public float breakForce = 5f;  // unit of force to break vase
    // 1 = easily broken   20 = basically indestructible
    public float shardRemove = 4f; // Amount of time before deleting the shards of the broken vase

    public RegenerateVase spawner; // to create a new vase
    private bool hasBroken = false;

    [Header("Shatter / Explosion")]
    public float explosionForce = 350f; // Impulse strength
    public float explosionRadius = 1.2f;
    public float upwardModifier = 0.2f; // Slight lift
    public float randomTorque = 10f;    // Spin
    public bool useImpactPoint = true;

    private void OnCollisionEnter(Collision collision) {
        if (hasBroken) return;

        if (collision.relativeVelocity.magnitude > breakForce) {
            Vector3 center = transform.position;
            if (useImpactPoint && collision.contactCount > 0) {
                // use the point of impact as the center of the broken vase
                center = collision.GetContact(0).point;
            }
            Break(center, collision.relativeVelocity);  // "break" vase
        }
    }

    void Break(Vector3 explosionCenter, Vector3 impactVelocity) {
        if (hasBroken) return;
        hasBroken = true;
        // create the broken vase
        GameObject shards = Instantiate(brokenVase, transform.position, transform.rotation);
        shards.tag = "shard"; // used in Potion.cs to make sure shards aren't recolored
        foreach (Transform child in shards.transform)
        {
            child.tag = "shard";
        }

        // Push shards away from center
        Rigidbody[] shardBodies = shards.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in shardBodies)
        {
            // Add explosion impulse
            rb.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, upwardModifier, ForceMode.Impulse);

            rb.AddForce(impactVelocity.normalized * (explosionForce * 0.1f), ForceMode.Impulse);

            // Add random spin
            rb.AddTorque(Random.insideUnitSphere * randomTorque, ForceMode.Impulse);
    }

        // Check if a spawner exists
        // This should be attatched to an empty obj in the scene
        if (spawner != null) {
            spawner.OnVaseBroken();
        }
        
        // destroy non-broken version of the vase
        Destroy(gameObject);

        // remove vase shards after a specific amount of time
        Destroy(shards, shardRemove);
    }
}
