using TMPro;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the prompt message
    [SerializeField]
    public TextMeshProUGUI promptText;

    private float interactionDistance;
    [SerializeField]
    public PickUpScript pickUpScript;

    private LayerMask interactableLayerMask;

    private Interactable interactable;
    private Interactable previousInteractable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionDistance = pickUpScript.maxHoldDistance;
        interactableLayerMask = LayerMask.GetMask("Interactable", "Potion");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance, interactableLayerMask))
        {
            interactable = hit.collider.GetComponent<Interactable>();
        }
        else
        {
            interactable = null;
        }

        if (pickUpScript.IsHoldingObject())
        {
            interactable = null;
        }
        if (interactable == previousInteractable)
            return;

        previousInteractable = interactable;

        if (interactable != null)
        {
            promptText.text = interactable.PromptMessage;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
