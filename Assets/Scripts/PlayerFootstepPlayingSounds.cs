using UnityEngine;

public class PlayerFootstepPlayingSounds : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepSound;
    public float stepDelay = 0.5f;
    public CharacterController controller;
    private float stepTimer;

    // Update is called once per frame
    void Update()
    {
        if(controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if(stepTimer <= 0)
            {
                // Makes sure walking sounds don't overlap over each other
                footstepSource.PlayOneShot(footstepSound);
                stepTimer = stepDelay;
            }
        }
        
    }
}
