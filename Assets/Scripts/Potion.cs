using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public Potions potionType = Potions.Bounce;
    public float effectRadius = 20;
    public PhysicsMaterial bounceMaterial;
    public GameObject particleEffect;
    public Material effectedObjectMaterial;
    private bool isQuitting = false;
    private void OnDestroy()
    {
        if (isQuitting && Application.isPlaying)
        {
            return;
        }

        if (potionType == Potions.Bounce)
        {
            List<GameObject> propsInRange = FindPropsInRadius(this.transform.position);
            foreach (GameObject obj in propsInRange)
            {
                if (obj.CompareTag("shard"))
                {
                    // dont add any effects to shards from exploding breakable potion bottle
                    continue;
                }

                // add bounce script
                obj.AddComponent<Bouncy>();

                // Add custom physics material for bounciness and no friction
                //Collider collider = obj.GetComponent<Collider>();
                //collider.material = bounceMaterial;

                // Change rigidbody settings to prevent going through walls and smooth out movement
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                // Apply slightly random upwards velocity to start bouncing
                rb.linearVelocity = new Vector3(Random.Range(0, 20), 20, Random.Range(0, 20));

                // Set object to red
                obj.GetComponent<Renderer>().material = effectedObjectMaterial;
                
            }
            // Play particle effect
            ExplodeEffect();
        }
        
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public List<GameObject> FindPropsInRadius(Vector3 center)
    {
        // Get all colliders within the sphere with radius range
        Collider[] hitColliders = Physics.OverlapSphere(center, effectRadius);
        List<GameObject> propsInRange = new List<GameObject>();

        // Iterate through the colliders and get their Rigidbodies
        foreach (Collider hitCollider in hitColliders)
        {
            // Add to list if a Rigidbody is present on the object
            if (hitCollider.TryGetComponent<Rigidbody>(out var rb))
            {
                propsInRange.Add(hitCollider.gameObject);
            }
        }
        return propsInRange;
    }

    public void ExplodeEffect()
    {
        // Spawn the particles at the current position
        GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);

        // Ensure the particle system destroys itself after finishing
        Destroy(effect, 2f);
    }



    public enum Potions
    {
        Bounce,
        Levitation
    }
}
