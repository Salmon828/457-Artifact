using UnityEngine;

public class Boss : MonoBehaviour
{

    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 5f; // Speed at which the boss rotates to face the player
    public bool smoothRotation = true; // Whether to use smooth rotation or instant rotation
    public bool lockVerticalAxis = true; // Keep enemy upright by locking vertical rotation
    public bool isAttacking = false; // Flag to indicate if the boss is currently attacking
    public Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
            {
            // Try to find the player by tag if not assigned
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Please assign the player Transform or tag an object as 'Player'.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        isAttacking = stateInfo.IsName("Armature|ChargeAttack");
        if (player == null || isAttacking) return; 

        Vector3 directionToPlayer = player.position - transform.position;
        
        if (lockVerticalAxis)
        {
            directionToPlayer.y = 0; // Lock vertical rotation
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(0, -90f, 0); // Multiply by 90 degrees to face the player correctly

        if (smoothRotation)
        {
            // Smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Instantly rotate to face the player
            transform.rotation = targetRotation;
        }
    }
}
