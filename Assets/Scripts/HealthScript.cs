using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text playerHealth;
    public int health = 150;
    public int maxHealth = 0;
    public bool sheildActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(sheildActive);
        playerHealth.text = health + " / " + maxHealth;
        healthBar.value = (float)health / (float)maxHealth;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sheildActive == false)
            {
                sheildActive = true;
                Invoke("coolDown", 1);
            }
        }
    }
    public void coolDown()
    {
        sheildActive = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (sheildActive == true)
            {
                //take no damage
            }
            //you take damage
            if (health > 0)
            {
                Debug.Log("you took damage");
                health = health - 10;
            }
        }
        if (other.CompareTag("Heal"))
        {
            //you heal damage
            if (health > 0 && health < 90)
            {
                Debug.Log("you Healed");
                health = health + 10;
                Destroy(GameObject.FindWithTag("Heal"));
            }
        }
    }
}
