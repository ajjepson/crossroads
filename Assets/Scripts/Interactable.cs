using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Interactable : MonoBehaviour
{
    public GameObject UIPopUp;
    public string interactionText = "Press E to interact";
    private bool isPlayerInRange;
    public TMP_Text popupText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show UI pop-up
            if (UIPopUp != null)
                UIPopUp.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide UI pop-up
            if (UIPopUp != null)
                UIPopUp.SetActive(false);
        }
    }
    void Interact()
    {
        if (UIPopUp != null)
        {
            UIPopUp.SetActive(true);
        }

        if (popupText != null)
        {
            popupText.text = interactionText;
        }
        Debug.Log("Item picked up!");
        Destroy(gameObject);

    }
}
