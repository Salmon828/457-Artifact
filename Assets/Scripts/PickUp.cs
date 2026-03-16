using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    public float holdForce = 150f; //spring force pulling held object toward holdPos, how fast it snaps to the hold position
    public float holdDamping = 15f; //damping applied to held object velocity to reduce oscillation
    public float maxHoldDistance = 3f; //distance at which the grab is auto-released
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private float originalDrag; //stored drag value so we can restore it on drop
    private RigidbodyConstraints originalConstraints; //stored constraints so we can restore them on drop

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {

    }
    void Update()
    {
        if (IsPickUpPressed()) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            RotateObject();
            if (IsThrowPressed() && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            originalDrag = heldObjRb.linearDamping;
            originalConstraints = heldObjRb.constraints;
            heldObjRb.useGravity = false; //disable gravity so we don't have to fight it with force
            heldObjRb.linearDamping = holdDamping; //use drag to dampen oscillation while held
            heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            heldObjRb.interpolation = RigidbodyInterpolation.Interpolate; // smooth out movement of held objects (prevent jittering)
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObjRb.useGravity = true;
        heldObjRb.linearDamping = originalDrag;
        heldObjRb.constraints = originalConstraints;
        heldObj = null; //undefine game object
        heldObjRb.interpolation = RigidbodyInterpolation.None;
    }
    void FixedUpdate()
    {
        if (heldObj == null) return;

        Vector3 toTarget = holdPos.position - heldObj.transform.position;
        float distance = toTarget.magnitude;

        //auto-release if the object is too far from the hold position (e.g. blocked by geometry)
        if (distance > maxHoldDistance)
        {
            DropObject();
            return;
        }

        //apply spring force toward holdPos; drag on the rigidbody handles damping
        heldObjRb.AddForce(toTarget * holdForce);
    }
    void RotateObject()
    {
        if (IsRotateHeld())//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            var delta = GetMouseDelta();
            float XaxisRotation = delta.x * rotationSensitivity;
            float YaxisRotation = delta.y * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }

    // Input abstraction helpers that should work with both input systems 
    private bool IsPickUpPressed()
    {
        if (Keyboard.current != null)
            return Keyboard.current.eKey.wasPressedThisFrame;

        return Input.GetKeyDown(KeyCode.E);
    }

    private bool IsThrowPressed()
    {
        if (Mouse.current != null)
            return Mouse.current.leftButton.wasPressedThisFrame;

        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private bool IsRotateHeld()
    {
        if (Keyboard.current != null)
            return Keyboard.current.rKey.isPressed;
        return Input.GetKey(KeyCode.R);
    }

    private Vector2 GetMouseDelta()
    {
        if (Mouse.current != null)
        {
            var d = Mouse.current.delta.ReadValue();
            return new Vector2(d.x, d.y);
        }
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObjRb.useGravity = true;
        heldObjRb.linearDamping = originalDrag;
        heldObjRb.constraints = originalConstraints;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
