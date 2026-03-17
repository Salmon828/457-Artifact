using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Material _baseMaterial;
    private Vector3 _baseSize;
    private Vector3 _baseExtents; // used for checking if there is room to unshrink
    private Collider _collider;
    private bool inStand = false;
    private Rigidbody rb;

    // Keeps track of all active potion materials in order
    private List<PotionEntry> _potionStack = new List<PotionEntry>();


    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _baseMaterial = _renderer.material;
        _baseSize = gameObject.transform.localScale;
        _collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        // Store actual size of mesh at start
        // renderer.bounds.extents is world-space, so divide by current size 
        _baseExtents = new Vector3(
            _renderer.bounds.extents.x / transform.localScale.x,
            _renderer.bounds.extents.y / transform.localScale.y,
            _renderer.bounds.extents.z / transform.localScale.z
        );
    }

    public void AddEffect(Material newMat, float shrinkFactor)
    {
        _potionStack.Add(new PotionEntry(newMat, shrinkFactor));
        UpdateVisuals();
    }


    // This is for the crystal ball to stop it from changing sizes after getting
    // placed in its stand
    public void setInStand()
    {
        inStand = true;
    }

    public void RemoveEffect(Material mat, float shrinkFactor)
    {
        _potionStack.Remove(new PotionEntry(mat, shrinkFactor));
        UpdateVisuals();
    }

    public void ClearAllPotions()
    {
        _potionStack.Clear();
        _renderer.material = _baseMaterial; 
        transform.localScale = _baseSize;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        Vector3 targetScale = _baseSize;
        Material targetMat = _baseMaterial;

        // Multiply all active shrink multipliers together
        foreach (var potion in _potionStack)
        {
            targetScale *= potion.shrinkFactor;
            targetMat = potion.mat; // Color will be set by the last material in the list
        }

        _renderer.material = targetMat;

        // If crystal ball is in stand this indicates puzzle is solved, stop doing any
        // materialManager logic
        if (inStand)
        {
            transform.localScale = Vector3.one;
            return;
        }

        if (targetScale != transform.localScale)
        {
            // If growing, check for obstacles
            if (targetScale.magnitude > transform.localScale.magnitude)
            {
                // no room to expand, nudge upwards
                Nudge(targetScale);

            }
            transform.localScale = targetScale;
        }

        if (rb != null)
        {
            rb.WakeUp();
            rb.MovePosition(transform.position);
        }
        
    }

    private void Nudge(Vector3 targetScale)
    {
        Vector3 targetExtents = Vector3.Scale(_baseExtents, targetScale);
        int layerMask = ~LayerMask.GetMask("Potion", "Player"); // Don't nudge away from player or other potions

        // Find colliders that would be hit at the new scale
        Collider[] hitColliders = Physics.OverlapBox(transform.position, targetExtents, transform.rotation, layerMask);

        foreach (var hit in hitColliders)
        {
            // Skip this object
            if (hit.gameObject == gameObject) continue;

            Vector3 direction;
            float distance;

            // Calculate how to get out of the other collider
            bool overlapped = Physics.ComputePenetration(
                _collider, transform.position, transform.rotation, // Current collider
                hit, hit.transform.position, hit.transform.rotation, // The object we would hit
                out direction, out distance
            );

            if (overlapped)
            {
                // Move the object by the distance required to escape the collision + a buffer 
                transform.position += direction * (distance + 0.01f);
            }
        }
    }

    [System.Serializable]
    public class PotionEntry
    {
        public Material mat;
        public float shrinkFactor;
        public PotionEntry(Material mat, float shrinkFactor)
        {
            this.mat = mat;
            this.shrinkFactor = shrinkFactor;
        }
        public override bool Equals(object obj)
        {
            if (obj is PotionEntry other)
            {
                // Check if material and shrink factor is the same
                return this.mat.Equals(other.mat) && this.shrinkFactor == other.shrinkFactor;
            }
            return false;
        }

        // Required since equals was overriden
        public override int GetHashCode()
        {
            return (mat, shrinkFactor).GetHashCode();
        }
    }

}
