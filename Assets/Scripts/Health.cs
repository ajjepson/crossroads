using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;

    public FloatingHealthBar floatingHealthBar;

    void Start()
    {
        health = maxHealth;
        UpdateUI();
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
        if (floatingHealthBar != null)
            floatingHealthBar.UpdateBar(health, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }






}