using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    public GameObject timelineToPlay; // assign in inspector, set to null if no timeline should play
    public AudioSource cutsceneSound; // plays the puzzle solved jingle
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("EscapeBall")) {

            // Handle material changes
            if (other.gameObject.TryGetComponent<MaterialManager>(out var materialManager))
            {
                materialManager.ClearAllPotions();
            }
            
            // Remove all shrink effects used to solve the puzzle
            Shrink[] activeShrinks = other.GetComponents<Shrink>();
            foreach (Shrink s in activeShrinks)
            {
                s.StopAllCoroutines();
                //s.undoShrink();
                Destroy(s);
            }

            // Disable physics, remove rigidbody, parent the stand, setInStand to fix size and prevent materialManager changes
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.gameObject.isStatic = true;
            other.gameObject.transform.SetParent(transform.parent, true);
            other.gameObject.transform.localPosition = new Vector3(0, 0.357f, 0);
            materialManager.setInStand();
            
            // Apply glow material to indiciate puzzle was solved
            materialManager.AddEffect(Resources.Load<Material>("Travis/GlowingOrb"), 1f);
            cutsceneSound.Play();

            // Play cutscene if assigned
            if (timelineToPlay != null)
            {
                timelineToPlay.SetActive(true);
            }
        }
    }
}
