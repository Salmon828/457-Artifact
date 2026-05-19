using System;
using UnityEngine;

public class OpenItem : Interactable
{
    public Boolean isLocked = false;
    private string message;
    public AudioSource unlockSound;

    private void Start()
    {
        if (isLocked)
        {
            message = "Press E to open (Locked)";
        }
        else
        {
            message = "Press E to open";
        }
    }

    public void Unlock()
    {
        isLocked = false;
        message = "Press E to open";
        if (unlockSound != null)
            unlockSound.Play();
    }
    protected override string DefaultMessage => message;
}
