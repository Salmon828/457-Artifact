using UnityEngine;

// Abstract class for interactable prompts, allows for a custom message to be set for one off cases. Generally a certain message type category will be used.
public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private string customMessage;
    public string PromptMessage =>
        string.IsNullOrEmpty(customMessage) ? DefaultMessage : customMessage;

    protected abstract string DefaultMessage { get; }
}
