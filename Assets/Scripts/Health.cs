using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
       public float health = 100f;
    public float maxHealth = 100f;
   
    public Image healthBar;
    public TextMeshProUGUI healthText;


    void Start()
    {
        health = maxHealth;
        UpdateHealthBarUI();
    }

    void Update()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        }

        if (healthText != null)
        {
            healthText.text = $"{gameObject.name} HP: {Mathf.RoundToInt(health)}";
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBarUI(); 

       healthBar.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        Destroy(gameObject);
    }
    void UpdateHealthBarUI()
    {
        float percent = health / maxHealth;

     
        if (healthBar != null)
            healthBar.fillAmount = percent;

     
        if (healthText != null)
            healthText.text = $"{gameObject.name} HP: {Mathf.RoundToInt(health)}";

        // Color change 
        if (healthBar != null)
        {
            if (percent > 0.5f)
                healthBar.color = Color.green;
            else if (percent > 0.25f)
                healthBar.color = Color.yellow;
            else
                healthBar.color = Color.red;
        }  }
}
