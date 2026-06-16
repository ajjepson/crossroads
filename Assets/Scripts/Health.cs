using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;

    public Image healthBar;
    public TextMeshProUGUI healthText;
   


    void Start()
    {
        health = maxHealth;
        UpdateUI();


    }

    void Update()
    {
        
      

    }
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage: " + damage);
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();

        if (health <= 0)
        {
            Die();
        }

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
        Destroy(gameObject);
    }
    void Awake()
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<UnityEngine.UI.Image>();

        if (healthText == null)
            healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }






}