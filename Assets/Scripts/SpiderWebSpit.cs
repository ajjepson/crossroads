using UnityEngine;

public class SpiderWebSpit : MonoBehaviour
{
    public float webSpitSpeed = 30;
    private Rigidbody rigidbod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbod = GetComponent<Rigidbody>();
        rigidbod.angularVelocity = transform.forward * webSpitSpeed;
        Destroy(gameObject, 7f);


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if sheild is not active

            //player takes damage
            //player speed decreases for 3? seconds
            Destroy(gameObject);
        }
        if (other.CompareTag("archer"))
        {
            //player takes damage
            //player speed decreases for 3? seconds
            Destroy(gameObject);
        }
    }
}
