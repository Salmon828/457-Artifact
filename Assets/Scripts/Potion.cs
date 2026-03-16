using NUnit.Framework;

using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public Potions potionType = Potions.Bounce;
    public float effectRadius = 20;
    public GameObject particleEffect;

    private bool isQuitting = false;

    private void OnDestroy()
    {
        if (isQuitting && Application.isPlaying)
        {
            return;
        }

        List<GameObject> propsInRange = FindPropsInRadius(this.transform.position);
        foreach (GameObject obj in propsInRange)
        {
            if (obj.CompareTag("shard"))
            {
                // dont add any effects to shards from exploding breakable potion bottle
                continue;
            }


            if (potionType == Potions.Bounce)
            {
                // add bounce script
                obj.AddComponent<Bouncy>();

            }
            else if (potionType == Potions.Shrink)
            {
                // add shrink script
                obj.AddComponent<Shrink>();
            }
            else
            {
                // add levitate script
                obj.AddComponent<Levitate>();
            }
        }

        // Play particle effect
        ExplodeEffect();

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
            // Add to list if a Rigidbody is present on the object or if its an elevator
            if (hitCollider.TryGetComponent<Rigidbody>(out var rb) || hitCollider.TryGetComponent<Elevator>(out var e))
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
        Levitate,
        Shrink
    }
}
