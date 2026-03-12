using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public float bounceSpeedBoost = 1.1f; // 1.1 = 10% faster each bounce
    public float maxSpeed = 10f; // stop object from spazzing as bounce keeps multiplying speed
    public float minVelocity = 0.2f; // Prevents the ball from getting stuck in a slow roll

    private Rigidbody rb;
    private Vector3 lastFrameVelocity;
    void Start() => rb = GetComponent<Rigidbody>();

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
        //rb.linearVelocity = direction * newSpeed;
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
}
