using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;

<<<<<<< HEAD
    public FloatingHealthBar floatingHealthBar;
=======
    public Image healthBar;
    public TextMeshProUGUI healthText;
   

>>>>>>> origin/main

    void Start()
    {
        health = maxHealth;
        UpdateUI();
<<<<<<< HEAD
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage: " + damage);

=======


    }

    void Update()
    {
        
      

    }
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage: " + damage);
>>>>>>> origin/main
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();

        if (health <= 0)
        {
            Die();
        }
<<<<<<< HEAD
    }

    void UpdateUI()
    {
        if (floatingHealthBar != null)
            floatingHealthBar.UpdateBar(health, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }
=======

    }
 
    void UpdateUI()
    {
        float percent = health / maxHealth;

        if (healthBar != null)
            healthBar.fillAmount = percent;

        if (healthText != null)
            healthText.text = $"{gameObject.name} HP: {Mathf.RoundToInt(health)}";
    }


    void Die()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.currentEnemies--;
        }

        Destroy(gameObject);
    }
    void Awake()
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<UnityEngine.UI.Image>();

        if (healthText == null)
            healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }
>>>>>>> origin/main






}