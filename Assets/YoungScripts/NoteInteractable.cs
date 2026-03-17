using UnityEngine;
using UnityEngine.InputSystem;

public class NoteInteractable : MonoBehaviour
{
    public Transform playerCameraTransform;
    public float interactRange = 4f;
    public NoteUI noteUI;
    public Sprite noteSprite;

    private void Update()
    {
        if (noteUI != null && noteUI.IsOpen()) return;

        bool interactPressed = false;

        if (Keyboard.current != null)
            interactPressed = Keyboard.current.eKey.wasPressedThisFrame;
        else
            interactPressed = Input.GetKeyDown(KeyCode.E);

        if (!interactPressed) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, interactRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (noteUI != null)
                {
                    noteUI.OpenNote(noteSprite);
                }
            }
        }
    }
}