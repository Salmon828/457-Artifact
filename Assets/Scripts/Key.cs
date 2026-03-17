using UnityEngine;

public class Key : MonoBehaviour
{
    public bool wasHeld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wasHeld = false;
        
    }

    public void setWasHeld(bool value) { wasHeld = value; }
    public bool getWasHeld() { return wasHeld; }

    private void OnTriggerEnter(Collider other)
    {
        if (wasHeld && other.TryGetComponent<Door>(out var door))
        {
            door.ToggleDoor();
            Destroy(this.gameObject);
        }
    }
}
