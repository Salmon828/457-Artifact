using UnityEngine;
using UnityEngine.InputSystem; // new input system

// Attatched to object you want to make draggable
// for vase, attatch to a non-broken vase
public class Dragable : MonoBehaviour {
    private Rigidbody rb;
    private bool isDragging = false;
    public float holdDistance = 5f;  // how close obj is to camera when picked up
    public float throwMultiplier = 0.5f;  // how much force is multiplied when obj is thrown
    private Vector3 lastPos;
    private Vector3 velocity;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        // *Commented out lines was using the old unity input system? But I got error messages so I believe this 
        // should be using the new input system, but I left the old code in case we needed it

        //if (Input.GetMouseButtonDown(0)) {
        if(Mouse.current.leftButton.wasPressedThisFrame){
            // Object was clicked, see if we can pick it up
            TryPickup();
        }

        //if (Input.GetMouseButtonUp(0)) {
        if(Mouse.current.leftButton.wasReleasedThisFrame){
            // Object was let go, drop it
            Drop();
        }

        if (isDragging) {
            // Object is currently being dragged (aka left click is being held)
            Drag();
        }
    }


    void TryPickup() {
        //Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);  // old input system
        
        // // Save for potential use later, with movement code
        // Transform camera = camera.main.transform;
        // Ray r = new Ray(camera.position, camera.forward);


        // Connects to MainCamera, grabs the mouse's position and converts into a 3d ray
        // going from the camera into the scene
        Ray r = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit; // Will store info about what the ray r hits

        if(Physics.Raycast(r, out hit)){  // if r collides with something
            if(hit.transform == transform) {  // if the object r collided with is the target obj (this object)
                isDragging = true;
                rb.useGravity = false; // Object isn't affected by gravity anymore
                rb.linearVelocity = Vector3.zero;
                lastPos = transform.position;  // used later to calculate velocity
            }
        }
    }

    void Drag() {
        //Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance));
        //transform.position = mousePoint;

        Vector2 mousepos = Mouse.current.position.ReadValue();  // get mouse's current position
        Vector3 scenePos = new Vector3(mousepos.x, mousepos.y, holdDistance);  // includes depth/how far from camera obj should be
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scenePos); // converts screenPos to a 3d world position

        
        // // Save for potential use later, with movement code
        // Transform camera = camera.main.transform;
        
        // // Position obj holdDistance units in front of the camera
        // Vector3 holdPoint = camera.position + camera.forward * holdDistance;
        // // holdPoint replaces worldPoint in rb.MovePosition and lastPos = 
        
        //Generate velocity
        Vector3 distanceMoved = worldPoint - lastPos;
        velocity = distanceMoved / Time.deltaTime;
        lastPos = worldPoint;

        rb.MovePosition(worldPoint);  // moves obj to the given position

    }

    void Drop() {
        isDragging = false;
        rb.useGravity = true;

        // Apply throwing force
        rb.linearVelocity = velocity * throwMultiplier;
    }
    
}