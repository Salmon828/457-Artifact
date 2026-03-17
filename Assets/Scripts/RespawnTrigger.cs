using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Transform respawnPoint;
    public Boss boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp"))
        {
            BreakableVase vase = other.GetComponent<BreakableVase>();
            if (vase != null)
            {
                vase.BreakFromTrigger();
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (respawnPoint != null)
            {
                CharacterController cc = other.GetComponent<CharacterController>();
                cc.enabled = false;
                other.transform.position = respawnPoint.position;
                cc.enabled = true;
                boss.ResetPosition();
            }
        }
    }
}
