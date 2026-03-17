using UnityEngine;

public class P2BarrelPuzzManager : MonoBehaviour
{
    // Checks for correct sequence of barrels clicked
    public GameObject activationObject; // the thing that happens after solving the puzz
    public int[] correctSequence = {4, 5, 7};
    private int progress = 0;  // can't this be a bool???

    public void BarrelClicked(int barrelID)
    {
        if(barrelID == correctSequence[progress])
        {
            // player clicked the correct code so far
            progress++;
            Debug.Log("Player clicked a correct barrel! Progress = " + progress);
            if(progress == correctSequence.Length)
            {
                // player clicked the entire password, puzzle is solved
                PuzzleSolvedProtocol();
                progress = 0;
            }
        } else
        {
            // reset progress
            progress = 0;
        }
    }

    public void PuzzleSolvedProtocol()
    {
        activationObject.SetActive(true);
    }

}
