using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float lifetime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}
