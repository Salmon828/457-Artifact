using UnityEngine;
using UnityEngine.InputSystem;

public class CrystalOrbInteractable : MonoBehaviour
{
    public Transform playerCameraTransform;
    public float interactRange = 4f;
    public int orbIndex;
    public CrystalPuzzleUI puzzleUI;
    public NoteUI noteUI;

    private void Update()
    {
        if (puzzleUI != null && puzzleUI.IsOpen()) return;
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
            CrystalOrbInteractable hitOrb = hit.collider.GetComponentInParent<CrystalOrbInteractable>();

            if (hitOrb == this && puzzleUI != null)
            {
                puzzleUI.OpenPanel(this);
            }
        }
    }
}