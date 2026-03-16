using System.Collections;
using UnityEngine;

// CITATION: Followed tutorial: https://www.youtube.com/watch?v=smlgtS07jaQ

public class Door : MonoBehaviour
{
    public GameObject doorToRotate;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool openOnStart = false;
    
    private bool isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCorotune;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _closedRotation = doorToRotate.transform.rotation;
        _openRotation = Quaternion.Euler(doorToRotate.transform.eulerAngles + new Vector3(0, openAngle, 0));
        if (openOnStart) { ToggleDoor(); }
    }

    public void ToggleDoor()
    {
        if (_currentCorotune != null) StopCoroutine(_currentCorotune);
        _currentCorotune = StartCoroutine(ToggleDoorCoroutine());
    }

    private IEnumerator ToggleDoorCoroutine()
    {
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            doorToRotate.transform.rotation = Quaternion.Lerp(doorToRotate.transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
