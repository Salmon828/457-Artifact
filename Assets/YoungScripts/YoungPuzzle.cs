using UnityEngine;

public class YoungPuzzle : MonoBehaviour
{
    public string acceptedItemId = "bookshelf_book";
    public Transform snapPoint;
    public YoungSlidingWall wallToOpen;
    public PickUpScript pickUpScript;

    private bool solved = false;

    private void OnTriggerStay(Collider other)
    {
        if (solved) return;

        YoungBookObject item = other.GetComponent<YoungBookObject>();
        if (item == null) return;
        if (item.isPlaced) return;
        if (item.itemId != acceptedItemId) return;
        if (pickUpScript == null) return;
        if (pickUpScript.holdPos == null) return;

        bool isStillHeld = other.transform.parent == pickUpScript.holdPos;

        // Only place after the player has dropped the book
        if (!isStillHeld)
        {
            PlaceBook(other.gameObject, item);
        }
    }

    private void PlaceBook(GameObject book, YoungBookObject item)
    {
        solved = true;
        item.isPlaced = true;

        Rigidbody rb = book.GetComponent<Rigidbody>();
        Collider[] cols = book.GetComponentsInChildren<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        book.transform.SetParent(snapPoint);
        book.transform.position = snapPoint.position;
        book.transform.rotation = snapPoint.rotation;

        foreach (Collider col in cols)
        {
            col.enabled = false;
        }

        if (wallToOpen != null)
        {
            wallToOpen.OpenWall();
        }
    }
}