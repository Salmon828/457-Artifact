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
    public UnityEvent onLeverPull;

    private void Start()
    {
        _unpulledRotation = lever.transform.localRotation;

        Quaternion offset = Quaternion.Euler(pullAngle, 0, 0);
        _pulledRotation = _unpulledRotation * offset;
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
        switchVal = !switchVal;
        Quaternion targetRotation = switchVal ? _pulledRotation : _unpulledRotation;


        onLeverPull.Invoke();
        if (spawner != null && switchVal)
        {
            spawner.GetComponent<RegenerateVase>().enabled = switchVal;
        }

        while (Quaternion.Angle(lever.transform.localRotation, targetRotation) > 0.01f)
        {
            lever.transform.localRotation = Quaternion.Lerp(lever.transform.localRotation, targetRotation, Time.deltaTime * pullSpeed);
            yield return null;
        }

        lever.transform.localRotation = targetRotation;
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
