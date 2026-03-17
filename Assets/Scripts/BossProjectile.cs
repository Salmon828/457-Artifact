using System.Collections;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float knockbackSpeed = 10f;
    public float knockbackDuration = 0.3f;
    public float explosionRadius = 3f;
    public GameObject explosionEffectPrefab;

    private Vector3 _start;
    private Vector3 _target;
    private float _arcHeight;
    private float _duration;
    private float _elapsed;
    private bool _exploded = false;

    public void Launch(Vector3 start, Vector3 target, float arcHeight, float duration)
    {
        _start     = start;
        _target    = target;
        _arcHeight = arcHeight;
        _duration  = duration;
        _elapsed   = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Boss"))
            Explode();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Boss"))
            Explode();
    }

    void Explode()
    {
        if (_exploded) return;
        _exploded = true;

        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Check for player in explosion radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                CharacterController controller = hit.GetComponent<CharacterController>();
                if (controller != null)
                {
                    Vector3 dir = hit.transform.position - transform.position;
                    dir.y = 0;
                    if (dir == Vector3.zero) dir = Vector3.forward;

                    // Hide projectile and apply knockback
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.enabled = false;
                    foreach (Collider c in GetComponentsInChildren<Collider>())
                        c.enabled = false;

                    StartCoroutine(ApplyKnockback(controller, dir.normalized));
                    return;
                }
            }
        }

        Destroy(gameObject);
    }

    IEnumerator ApplyKnockback(CharacterController controller, Vector3 direction)
    {
        float timer = 0f;
        while (timer < knockbackDuration)
        {
            controller.Move(direction * knockbackSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if (_exploded) return;

        _elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(_elapsed / _duration);

        // Lerp along the flat path, then add the arc on Y
        Vector3 pos = Vector3.Lerp(_start, _target, t);
        pos.y += _arcHeight * Mathf.Sin(t * Mathf.PI);

        transform.position = pos;

        // Face direction of travel
        if (t < 1f)
        {
            Vector3 next = Vector3.Lerp(_start, _target, t + 0.01f);
            next.y += _arcHeight * Mathf.Sin((t + 0.01f) * Mathf.PI);
            transform.rotation = Quaternion.LookRotation(next - pos) * Quaternion.Euler(0, 90f, 0);
        }
        else
        {
            Explode();
        }
    }
}
