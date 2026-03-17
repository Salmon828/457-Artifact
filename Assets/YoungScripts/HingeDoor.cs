using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    public float openAngle = -90f;
    public float openSpeed = 120f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool opening = false;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        if (!opening) return;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            openRotation,
            openSpeed * Time.deltaTime
        );
    }

    public void OpenDoor()
    {
        opening = true;
        //doorSound.Play();
    }
}