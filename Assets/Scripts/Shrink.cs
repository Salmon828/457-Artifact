using UnityEngine;
using System.Collections;
public class Shrink : MonoBehaviour
{
    public float shrinkFactor = 0.5f; // 0.5 = half the size
    
    public float effectDuration = 5f; // How long before object reverts
    private Material originalMaterial;
    private Vector3 originalScale; 
    private Rigidbody rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        // Set object to new mat but store original material to reset later
        originalMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = Resources.Load<Material>("Travis/Shrink");

        // Shrink obj
        originalScale = transform.localScale;
        transform.localScale = originalScale * shrinkFactor;

        StartCoroutine(undoShrink());
    }

    IEnumerator undoShrink()
    {
        // wait 
        yield return new WaitForSeconds(effectDuration);

        // undo size change
        transform.localScale = originalScale;

        // undo material change
        GetComponent<Renderer>().material = originalMaterial;

        // remove shrink script
        Destroy(GetComponent<Shrink>());

    }
}
