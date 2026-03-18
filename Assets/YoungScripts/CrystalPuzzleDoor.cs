using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CrystalDoorInteractable : MonoBehaviour
{
    [Header("Interaction")]
    public Transform playerCameraTransform;
    public float interactRange = 4f;

    [Header("References")]
    public CrystalPuzzleUI puzzleUI;

    [Header("Feedback")]
    public TextMeshProUGUI feedbackText;
    public float feedbackDuration = 2f;

    [Header("Door Opening")]
    public string openTriggerName = "Open";

    public Transform doorToRotate;
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);
    public float rotateSpeed = 2f;

    public AudioSource unlockedSound; // door unlocking sound

    private bool isOpen = false;
    private Quaternion targetRotation;

    private void Start()
    {
        if (doorToRotate != null)
            targetRotation = doorToRotate.rotation * Quaternion.Euler(openRotation);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    private void Update()
    {
        if (isOpen) return;
        if (puzzleUI != null && puzzleUI.IsOpen()) return;

        bool interactPressed = false;

        if (Keyboard.current != null)
            interactPressed = Keyboard.current.eKey.wasPressedThisFrame;
        else
            interactPressed = Input.GetKeyDown(KeyCode.E);

        if (!interactPressed) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, interactRange))
        {
            CrystalDoorInteractable hitDoor = hit.collider.GetComponentInParent<CrystalDoorInteractable>();

            if (hitDoor == this)
                TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        if (puzzleUI == null)
        {
            ShowFeedback("Puzzle UI missing.");
            return;
        }

        if (puzzleUI.IsCorrectWord())
        {
            isOpen = true;
            ShowFeedback("The door unlocked.");
            if(unlockedSound != null && unlockedSound.clip != null)
                unlockedSound.Play();

            if (doorToRotate != null)
                StartCoroutine(RotateDoorOpen());
        }
        else
        {
            ShowFeedback("Wrong code. Try again.");
        }
    }

    private IEnumerator RotateDoorOpen()
    {
        while (Quaternion.Angle(doorToRotate.rotation, targetRotation) > 0.1f)
        {
            doorToRotate.rotation = Quaternion.Slerp(
                doorToRotate.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed
            );
            yield return null;
        }

        doorToRotate.rotation = targetRotation;
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        StopAllCoroutines();
        StartCoroutine(ShowFeedbackRoutine(message));
    }

    private IEnumerator ShowFeedbackRoutine(string message)
    {
        feedbackText.text = message;
        yield return new WaitForSeconds(feedbackDuration);
        feedbackText.text = "";
    }
}