using UnityEngine;
using System.Collections;
public class Bouncy : MonoBehaviour
{
    public float bounceSpeedBoost = 1.1f; // 1.1 = 10% faster each bounce
    public float maxSpeed = 10f; // stop object from spazzing as bounce keeps multiplying speed
    public float minVelocity = 0.2f; // Prevents the ball from getting stuck in a slow roll
    public float effectDuration = 5f; // How long before object reverts
    private Material originalMaterial;

    private Rigidbody rb;
    private Vector3 lastFrameVelocity;
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        // Set object to red but store original material to reset later
        originalMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = Resources.Load<Material>("Travis/Bouncy");

        // Change rigidbody settings to prevent going through walls and smooth out movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Apply slightly random upwards velocity to start bouncing
        rb.linearVelocity = new Vector3(Random.Range(0, 20), 20, Random.Range(0, 20));

        StartCoroutine(undoBounce());
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        // Get the contact normal
        Vector3 normal = collision.GetContact(0).normal;

       
        // 3. Reflect the PRE-IMPACT velocity across the surface normal
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, normal);

        // 4. Apply boost and re-assign
        // We calculate speed separately to ensure the direction stays clean
        float newSpeed = Mathf.Min(speed * bounceSpeedBoost, maxSpeed);

        // Ensure we don't drop below a minimum speed to prevent "sticking"
        rb.linearVelocity = direction * Mathf.Max(newSpeed, minVelocity);
    }

    void FixedUpdate()
    {
        lastFrameVelocity = rb.linearVelocity;
        // limit object speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    IEnumerator undoBounce()
    {
        // wait 
        yield return new WaitForSeconds(effectDuration);

        // undo rigidbody changes
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        // undo material change
        GetComponent<Renderer>().material = originalMaterial;

        // remove bounce script
        Destroy(GetComponent<Bouncy>());

    }
}
