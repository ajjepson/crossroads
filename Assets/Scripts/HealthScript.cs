using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text playerHealth;
    private int health = 150;
    public int maxHealth = 0;
    public bool sheildActive = false;
    //new
    //public float sheildStreanth = 2f;
    public float sheildLength = 3f;
    public float sheildNotActive = 4f;
    public bool canPlayerSheild = true;
    public Image sheildImage;
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
        if (Input.GetKeyDown(KeyCode.Space) && canPlayerSheild == true)
        {
            Debug.Log("holding sheild");
            StartCoroutine(SheildBlock());
            /*
            if (sheildActive == false)
            {
                sheildActive = true;
                //Invoke("coolDown", 1);
            }
            */
        }
    }
    private IEnumerator SheildBlock()
    {
        canPlayerSheild = false;
        sheildActive = true;
        //yield return new WaitForSeconds(sheildLength);

        float countUpTime = 0f;
        sheildImage.fillAmount = 1f;

        while (countUpTime < sheildLength)
        {
            countUpTime += Time.deltaTime;
            sheildImage.fillAmount = 1f - (countUpTime / sheildLength);
            yield return null;
        }
        sheildActive = false;
        //flip this to fill back up (replace countUpTime with countDownTime and sheildLength with sheildNotActive)
        float countDownTime = 0f;
        sheildImage.fillAmount = 0f;
        while (countDownTime < sheildNotActive)
        {
            countDownTime += Time.deltaTime;
            sheildImage.fillAmount = (countDownTime / sheildNotActive);
            yield return null;
        }
        sheildImage.fillAmount = 1f;
        canPlayerSheild = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (sheildActive)
            {
                //take no damage
                return;
            }
            //you take damage
            if (health > 0)
            {
                Debug.Log("you took damage");
                health -= 10;
            }
        }
        if (other.CompareTag("spider"))
        {
            if (sheildActive)
            {
                //take no damage
                return;
            }
            //you take damage
            if (health > 0)
            {
                Debug.Log("you took damage");
                health -= 15;
            }
        }

        if (other.CompareTag("Heal"))
        {
            //you heal damage
            if (health > 0 && health < 90)
            {
                Debug.Log("you Healed");
                health += 10;
                Destroy(GameObject.FindWithTag("Heal"));
            }
        }

    }
}
