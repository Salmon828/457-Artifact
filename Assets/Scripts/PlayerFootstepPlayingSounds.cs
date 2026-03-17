using UnityEngine;

public class PlayerFootstepPlayingSounds : MonoBehaviour
{
    // MAKE SURE STAYONAWAKE IS NOT CHECKED ON IN AUDIOSOURCE 
    // (or else unity will probably crash)
    public AudioSource footstepSource;
    public AudioClip footstepSound;
    public float stepDelay = 0.5f;
    public CharacterController controller;
    private float stepTimer;

    void Start()
    {
        Debug.Log("Footstep script running on " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        // if assignments are unassigned
        if(controller == null || footstepSource == null || footstepSound == null) {
            Debug.Log("something is unassigned");
            return;
        }

        //Only count horizontal movement (ignore velocity from jumping)
        Vector3 horizVel = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        // Only play the footstep sound when player is grounded and moving
        if(controller.isGrounded && horizVel.magnitude > 0.1f)   //controller.velocity.magnitude
        {
            stepTimer -= Time.deltaTime;
            if(stepTimer <= 0f)
            {
                if (!footstepSource.isPlaying)
                {
                    // Makes sure walking sounds don't overlap over each other
                    //footstepSource.PlayOneShot(footstepSound);
                    Debug.Log("STEP");
                    // footstepSource.clip = footstepSound;
                    // footstepSource.Play();
                }
                //Debug.Log("STEP");
                //stepTimer = stepDelay;
                stepTimer = Mathf.Max(stepDelay, 0.3f); // not too fast
            }
        } 
        // else
        // {
        //     // reset timer when not walking
        //     stepTimer = 0f;
        // }
        
    }
}
