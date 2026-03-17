using UnityEngine;

public class VasePlatform : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveSpeed = 1f;
    public float speedVariance = 0.3f;
    public float phaseOffset = 0f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private float actualSpeed;
    private float phase;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        phase = phaseOffset;
        PickNewSpeed();
    }

    void FixedUpdate()
    {
        phase += actualSpeed * Time.fixedDeltaTime;

        if (phase >= 2f * Mathf.PI)
        {
            phase -= 2f * Mathf.PI;
            PickNewSpeed();
        }

        float offset = Mathf.Sin(phase) * moveDistance;
        rb.MovePosition(startPosition + Vector3.up * offset);
    }

    void PickNewSpeed()
    {
        actualSpeed = moveSpeed + Random.Range(-speedVariance, speedVariance);
    }
}
