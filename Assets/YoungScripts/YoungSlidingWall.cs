using UnityEngine;

public class YoungSlidingWall : MonoBehaviour
{
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 targetPos;
    private bool opening = false;

    private void Start()
    {
        targetPos = transform.position + Vector3.down * moveDistance;
    }

    private void Update()
    {
        if (!opening) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    public void OpenWall()
    {
        Debug.Log("OpenWall called");
        opening = true;
    }
}