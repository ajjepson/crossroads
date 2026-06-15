using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class HealthScript : MonoBehaviour
{
    //for audio
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private AudioClip hurtAudio;
    [SerializeField] private AudioClip eatAudio;
    private AudioSource audioHealthSource;
    // audio
    [Header("Health")]
    public float health = 150f;
    public float maxHealth = 150f;

    [Header("UI")]
    public Image healthBarFill;
    public TMP_Text playerHealth;

    [Header("Shield")]
    public bool shieldActive = false;
    public bool canUseShield = true;
    public float shieldDuration = 3f;
    public float shieldCooldown = 4f;
    public Image shieldImage;

    [Header("Invincibility")]
    public float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;

    [Header("Damage Feedback")]
    public Image damageFlashImage;
    public AudioClip hitSound;
    private AudioSource audioSource;

    [Header("Knockback")]
    public string playerTag = "Player";

    void Start()
    {
        audioHealthSource = GetComponent<AudioSource>();
        health = maxHealth;
        UpdateUI();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerHealth != null)
            playerHealth.text = Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth);

        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;

        if (Input.GetKeyDown(KeyCode.Space) && canUseShield)
            StartCoroutine(ShieldRoutine());

        UpdateHealthColor();
    }

    // DAMAGE
    public void TakeDamage(float damage, Vector3 hitSource)
    {
        if (isInvulnerable) return;
        if (shieldActive) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();
        PlayDamageFeedback();

        //  TAG-BASED 
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            charcontrol knight = player.GetComponent<charcontrol>();
            if (knight != null)
                knight.ApplyKnockback(hitSource);

            ChaControl2 archer = player.GetComponent<ChaControl2>();
            if (archer != null)
                archer.ApplyKnockback(hitSource);
        }

        if (health <= 0)
            Die();

        StartCoroutine(InvincibilityFrames());
    }

    // SIMPLE DAMAGE
    public void TakeDamage(float damage)
    {
        TakeDamage(damage, transform.position - transform.forward);
    }

    void PlayDamageFeedback()
    {
        if (damageFlashImage != null)
            StartCoroutine(DamageFlash());

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    IEnumerator DamageFlash()
    {
        damageFlashImage.gameObject.SetActive(true);

        Color c = damageFlashImage.color;
        c.a = 0.4f;
        damageFlashImage.color = c;

        yield return new WaitForSeconds(0.05f);

        float t = 0f;

        while (t < 0.2f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0.4f, 0f, t / 0.2f);
            damageFlashImage.color = c;
            yield return null;
        }

        c.a = 0f;
        damageFlashImage.color = c;
        damageFlashImage.gameObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateUI();
    }

    IEnumerator ShieldRoutine()
    {
        canUseShield = false;
        shieldActive = true;

        float t = 0f;

        if (shieldImage != null)
            shieldImage.fillAmount = 1f;

        while (t < shieldDuration)
        {
            t += Time.deltaTime;
            if (shieldImage != null)
                shieldImage.fillAmount = 1f - (t / shieldDuration);

            yield return null;
        }

        shieldActive = false;

        float cd = 0f;

        while (cd < shieldCooldown)
        {
            cd += Time.deltaTime;
            if (shieldImage != null)
                shieldImage.fillAmount = cd / shieldCooldown;

            yield return null;
        }

        if (shieldImage != null)
            shieldImage.fillAmount = 1f;

        canUseShield = true;
    }

    IEnumerator InvincibilityFrames()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            TakeDamage(10f);
            audioHealthSource.clip = hurtAudio;
            audioHealthSource.Play();
        }

        if (other.CompareTag("spider"))
        {
            TakeDamage(15f);
            audioHealthSource.clip = hurtAudio;
            audioHealthSource.Play();
        }
        if (other.CompareTag("Boss"))
        {
            TakeDamage(20);
            audioHealthSource.clip = hurtAudio;
            audioHealthSource.Play();
        }
            //TakeDamage(10f, other.transform.position);

        if (other.CompareTag("spider"))
            TakeDamage(15f, other.transform.position);

        if (other.CompareTag("Boss"))
            TakeDamage(20f, other.transform.position);

        if (other.CompareTag("Heal"))
        {
            Heal(10f);
            audioHealthSource.clip = eatAudio;
            audioHealthSource.Play();
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        audioHealthSource.clip = deathAudio;
        audioHealthSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;

        if (playerHealth != null)
            playerHealth.text = Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth);
    }

    void UpdateHealthColor()
    {
        if (healthBarFill == null) return;

        float percent = health / maxHealth;

        if (percent > 0.5f)
            healthBarFill.color = Color.green;
        else if (percent > 0.25f)
            healthBarFill.color = Color.yellow;
        else
            healthBarFill.color = Color.red;
    }
}
