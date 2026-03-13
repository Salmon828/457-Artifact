using System.Collections;
using UnityEditor.Search;
using UnityEngine;
public class Levitate : MonoBehaviour
{
    public float moveSpeed = 15f;
    
    public float effectDuration = 5f; // How long before object reverts
    private Material originalMaterial;
    private Vector3 originalScale; 
    private Rigidbody rb;
    private Vector3 lastFrameVelocity;
    public float maxSpeed = 10f; // stop object from spazzing as bounce keeps multiplying speed

    void Start() 
    {
        rb = GetComponent<Rigidbody>();

        // Set object to new mat but store original material to reset later
        originalMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = Resources.Load<Material>("Travis/Levitate");

        StartCoroutine(undoLevitate());
    }

    private void FixedUpdate()
    {
        // Apply a continuous force upwards
        rb.AddForce(Vector3.up * moveSpeed);

        lastFrameVelocity = rb.linearVelocity;
        // limit object speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }


    IEnumerator undoLevitate()
    {
        // wait 
        yield return new WaitForSeconds(effectDuration);


        // undo material change
        GetComponent<Renderer>().material = originalMaterial;

        // remove shrink script
        Destroy(GetComponent<Levitate>());

    }
}
