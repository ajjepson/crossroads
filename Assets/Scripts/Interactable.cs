using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Interactable : MonoBehaviour
{
    public GameObject UIPopUp;
    public string interactionText = "Press E to interact";
    private bool isPlayerInRange;
    public TMP_Text popupText;
    public float spinSpeed = 100f;
    public float healAmount = 25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            HealthScript HealthScript = player.GetComponent<HealthScript>();
            if (HealthScript != null)
            {
                HealthScript.Heal(10f);
            }


        }
        Debug.Log("Item picked up!");
        Destroy(gameObject);

    }
}
