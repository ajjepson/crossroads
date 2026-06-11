using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ArcherHealth : MonoBehaviour
{
    [Header("UI")]
    public Image healthBarFill;
    public TMP_Text playerHealth;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }
    void UpdateUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

        if (playerHealth != null)
            playerHealth.text = currentHealth + " / " + maxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateHealthColor();


    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            TakeDamage(10);
        }
        else if (other.CompareTag("spider"))
        {
            TakeDamage(15);
        }
        else if (other.CompareTag("Boss"))
        {
            TakeDamage(20);
        }
        else if (other.CompareTag("Ice"))
        {
            Destroy(other.gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Archer took damage");

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Die()
    {
        Debug.Log("Archer died");
        gameObject.SetActive(false);
    }
    void UpdateHealthColor()
    {
        if (healthBarFill == null) return;

        float percent = (float)currentHealth / maxHealth;

        if (percent > 0.5f)
            healthBarFill.color = Color.green;
        else if (percent > 0.25f)
            healthBarFill.color = Color.yellow;
        else
            healthBarFill.color = Color.red;
    }


}
