using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PlayerHealth health = other.GetComponent<PlayerHealth>();
            //if (health != null)
            {
            //    health.TakeDamage(damage, true);
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Bullet"))
        {
            // Destroy(gameObject);
        }
    }
}
