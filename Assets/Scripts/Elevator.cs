using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour


{
    bool isMoving;
    bool reverse = true;
    public float moveDist;
    public float moveSpeed;

    private Vector3 floor1Vec;
    private Vector3 floor2Vec;
    GameObject platform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform = this.gameObject;
        floor1Vec = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z);
        floor2Vec = new Vector3(platform.transform.position.x, platform.transform.position.y - moveDist, platform.transform.position.z);
    }

    void OnElevator(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            isMoving = true;
            reverse = !reverse;
            
        }

        
    }

    public void ToggleFloor()
    {
        isMoving = true;
        reverse = !reverse;
        
    }

    void Update()
    {

        if (isMoving)
        {
            Vector3 currY = platform.transform.position;
            Vector3 targetY = reverse ? floor1Vec : floor2Vec;
            platform.transform.position = Vector3.MoveTowards(currY, targetY, moveSpeed * Time.deltaTime);
        }
    }
}
