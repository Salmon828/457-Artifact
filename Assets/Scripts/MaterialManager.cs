using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Material _baseMaterial;
    private Vector3 _baseSize;

    // This list keeps track of all active potion materials in order
    private List<Material> _effectStack = new List<Material>();

    // This list keeps track of all active shrink potion sizes in order
    private List<Vector3> _shrinkStack = new List<Vector3>();

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _baseMaterial = _renderer.material;
        _baseSize = gameObject.transform.localScale;
    }

    public void AddEffect(Material newMat, float shrinkFactor)
    {
        _effectStack.Add(newMat);
        if (_shrinkStack.Count > 0)
        {
            _shrinkStack.Add(_shrinkStack[_shrinkStack.Count - 1] * shrinkFactor);
        } else
        {
            _shrinkStack.Add(_baseSize * shrinkFactor);
        }

            UpdateVisuals();
    }


    public void RemoveEffect(Material mat)
    {
        if (_effectStack.Contains(mat))
        {
            _effectStack.Remove(mat);
            if (mat.Equals(Resources.Load<Material>("Travis/Shrink")))
            {
                _shrinkStack.RemoveAt(_shrinkStack.Count - 1);
            }
            
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_effectStack.Count > 0)
        {
            // Apply the most recent potion added to the list
            _renderer.material = _effectStack[_effectStack.Count - 1];

            // Set size
            gameObject.transform.localScale = _shrinkStack[_shrinkStack.Count - 1];
        }
        else
        {
            // No potions left? Go back to the original and destroy
            _renderer.material = _baseMaterial;
            gameObject.transform.localScale = _baseSize;
            Destroy(this);
        }
    }
   
}
