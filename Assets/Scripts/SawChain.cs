using UnityEngine;

public class SawChain : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 80f; // Degrees per second
    [SerializeField] private float upperBound = 50f; // Upper rotation limit in degrees
    [SerializeField] private float lowerBound = -60f; // Lower rotation limit in degrees
    private float currentRotation;
    [SerializeField] private int direction = 1;

    private void Awake()
    {
        currentRotation = Mathf.Clamp(currentRotation, lowerBound, upperBound);
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation += rotationSpeed * direction * Time.deltaTime;
        if (currentRotation >= upperBound)
        {
            currentRotation = upperBound;
            direction = -1;
        }
        else if (currentRotation <= lowerBound)
        {
            currentRotation = lowerBound;
            direction = 1;
        }

        transform.localRotation = Quaternion.Euler(currentRotation, 0f, 0f);
    }
}
