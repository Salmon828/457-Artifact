using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NoteUI : MonoBehaviour
{
    public GameObject notePanel;
    public Image noteImage;

    private bool isOpen = false;

    private void Start()
    {
        if (notePanel != null)
            notePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen) return;

        bool closePressed = false;

        if (Keyboard.current != null)
            closePressed = Keyboard.current.escapeKey.wasPressedThisFrame;
        else
            closePressed = Input.GetKeyDown(KeyCode.Escape);

        if (closePressed)
        {
            CloseNote();
        }
    }

    public void OpenNote(Sprite spriteToShow)
    {
        if (notePanel != null)
            notePanel.SetActive(true);

        if (noteImage != null)
            noteImage.sprite = spriteToShow;

        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        if (notePanel != null)
            notePanel.SetActive(false);

        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}