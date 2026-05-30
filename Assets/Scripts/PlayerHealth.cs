using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
       public float health = 100f;
    public float maxHealth = 100f;
    public Image healthBar;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
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
    }
    void UpdateHealthBarUI()
    {
        float percent = health / maxHealth;
        healthBar.fillAmount = percent;
        if (percent > 0.5f)
            healthBar.color = Color.green;
        else if (percent > 0.25f)
            healthBar.color = Color.yellow;
        else
            healthBar.color = Color.red;

    }
}
