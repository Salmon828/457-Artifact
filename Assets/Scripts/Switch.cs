using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public GameObject lever;

    private bool playerInRange = false;
    public bool switchVal = false;
    private Quaternion _unpulledRotation;
    private Quaternion _pulledRotation;
    private Coroutine _currentCorotune;
    private float pullAngle = 45f;
    private float pullSpeed = 2f;
    public GameObject spawner;

    private void Start()
    {
        _unpulledRotation = lever.transform.rotation;
        _pulledRotation = Quaternion.Euler(lever.transform.eulerAngles + new Vector3(pullAngle, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        { 
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


    private IEnumerator ToggleSwitchCoroutine()
    {
        Quaternion targetRotation = switchVal ? _unpulledRotation : _pulledRotation;
        switchVal = !switchVal;

        spawner.GetComponent<RegenerateVase>().enabled = switchVal;
        while (Quaternion.Angle(lever.transform.rotation, targetRotation) > 0.01f)
        {
            lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, targetRotation, Time.deltaTime * pullSpeed);
            yield return null;
        }

        lever.transform.rotation = targetRotation;
    }



    // Update is called once per frame
    void Update()
    {

        if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame && playerInRange)
        {
            StopAllCoroutines();
            StartCoroutine(ToggleSwitchCoroutine());
        }
    }
}
