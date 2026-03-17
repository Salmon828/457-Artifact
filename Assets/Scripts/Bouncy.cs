using UnityEngine;
using System.Collections;
public class Bouncy : MonoBehaviour
{
    public float bounceSpeedBoost = 1.1f; // 1.1 = 10% faster each bounce
    public float maxSpeed = 10f; // stop object from spazzing as bounce keeps multiplying speed
    public float minVelocity = 0.2f; // Prevents the ball from getting stuck in a slow roll
    public float effectDuration = 5f; // How long before object reverts

    private MaterialManager materialManager;
    private Rigidbody rb;
    private Vector3 lastFrameVelocity;
    private Elevator elevator;
    void Start() 
    {
        if (TryGetComponent<Rigidbody>(out rb))
        {
            // Change rigidbody settings to prevent going through walls and smooth out movement
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Apply slightly random upwards velocity to start bouncing
            rb.linearVelocity = new Vector3(Random.Range(0, 20), 20, Random.Range(0, 20));
        }

        // Handle material changes
        if (!TryGetComponent<MaterialManager>(out materialManager))
        {
            materialManager = gameObject.AddComponent<MaterialManager>();
        }
        materialManager.AddEffect(Resources.Load<Material>("Travis/Bouncy"), 1f);

        // Ignore this effect if attached obj is an elevator
        TryGetComponent<Elevator>(out elevator);

        StartCoroutine(undoBounce());
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore this effect if attached obj is an elevator
        if (elevator != null)
        {
            return;
        }

        // Get contact normal
        Vector3 normal = collision.GetContact(0).normal;

       
        // Reflect pre-impact velocity across surface normal
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, normal);

        // Apply boost and re-assign
        float newSpeed = Mathf.Min(speed * bounceSpeedBoost, maxSpeed);

        // Don't drop below a minimum speed to prevent sticking / sliding
        rb.linearVelocity = direction * Mathf.Max(newSpeed, minVelocity);
    }

    void FixedUpdate()
    {
        // Ignore this effect if attached obj is an elevator
        if (elevator != null)
        {
            return;
        }

        // limit object speed to prevent sticking / sliding
        lastFrameVelocity = rb.linearVelocity;
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    IEnumerator undoBounce()
    {
        // wait 
        yield return new WaitForSeconds(effectDuration);

        if (elevator == null)
        {
            // undo rigidbody changes, but leave detection mode to continuous so obj doesn't fall thru floor
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.None;
        }

        // undo material change
        materialManager.RemoveEffect(Resources.Load<Material>("Travis/Bouncy"), 1f);

        // remove bounce script
        Destroy(GetComponent<Bouncy>());

    }
}
