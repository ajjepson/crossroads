using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;

    public Image healthBar;
    public TextMeshProUGUI healthText;
    public float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;
    


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
        if (isInvulnerable) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBarUI();

        if (health <= 0)
        {
            Die();
        }

        StartCoroutine(InvincibilityFrames());
    }

    void Die()
    {
        Debug.Log("Player died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Hit by enemy!");
            TakeDamage(10f); // Damage amount
        }
    }
    private IEnumerator InvincibilityFrames()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
    }
   
    

}