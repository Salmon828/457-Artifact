using TMPro;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject winText; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("canPickUp")) {
            Debug.Log("You win!");
            winText.SetActive(true); // Show the win text
        }
    }
}
