using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArcherHealth : MonoBehaviour
{
    public Slider ArchhealthBar;
    public TMP_Text playerHealth;
    private int ArchHealth = 100;
    public int ArchersmaxHealth = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArchersmaxHealth = ArchHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.text = ArchHealth + " / " + ArchersmaxHealth;
        ArchhealthBar.value = (float)ArchHealth / (float)ArchersmaxHealth;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            //you take damage
            if (ArchHealth >= 0)
            {
                Debug.Log("archer took damage");
                ArchHealth -= 10;
            }
        }
        if (other.CompareTag("spider"))
        {
            if (ArchHealth >= 0)
            {
                Debug.Log("archer took damage");
                ArchHealth -= 15;
            }
        }
    }
}
