using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBoss : MonoBehaviour
{
    public Slider spiderhealthBar;
    public TMP_Text spiderHealthText;
    private int spiderHealth = 500;
    public int spiderMaxHealth = 0;
    public SwordSwing swordSwing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spiderMaxHealth = spiderHealth;
    }

    // Update is called once per frame
    void Update()
    {
        spiderHealthText.text = spiderHealth + " / " + spiderMaxHealth;
        spiderhealthBar.value = (float)spiderHealth / (float)spiderMaxHealth;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (swordSwing.canPlayerSwing == true)
        {
                Debug.Log("spider took slice damage");
                spiderHealth -= 10;
        }
        if (other.CompareTag("Arrows"))
        {
            Debug.Log("spider took arrow damage");
            spiderHealth -= 5;
        }
        if ( spiderHealth < 0)
        {
            //loads next level
        }
    }
}
