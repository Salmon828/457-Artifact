using UnityEngine;
using System.Collections;
public class Shrink : MonoBehaviour
{
    public float shrinkFactor = 0.5f; // 0.5 = half the size
    
    public float effectDuration = 5f; // How long before object reverts
    //private Material originalMaterial;
    private Vector3 originalScale; 
    private MaterialManager materialManager;

    void Start() 
    {
        // Handle material changes
        if (!TryGetComponent<MaterialManager>(out materialManager))
        {
            materialManager = gameObject.AddComponent<MaterialManager>();
        }

        // Shrink obj logic moved to materialManager to handle effect stacking
        materialManager.AddEffect(Resources.Load<Material>("Travis/Shrink"), shrinkFactor);

        StartCoroutine(undoShrink());
    }

    IEnumerator undoShrink()
    {
        // wait 
        yield return new WaitForSeconds(effectDuration);

        // undo material change
        materialManager.RemoveEffect(Resources.Load<Material>("Travis/Shrink"));

        // remove shrink script
        Destroy(GetComponent<Shrink>());
    }
}
