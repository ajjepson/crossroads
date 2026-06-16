using UnityEngine;

public class SpiderWebSpit : MonoBehaviour
{
    public float webSpitSpeed = 30;
    private Rigidbody rigidbod;
    private Transform chasePlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbod = GetComponent<Rigidbody>();
        //rigidbod.angularVelocity = transform.forward * webSpitSpeed;
        chasePlayer = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, 7f);


    }
    private void Update()
    {
        if (chasePlayer != null)
        {
            //player cant be found
            return;
        }
        Vector3 playerDirection = (chasePlayer.position - transform.position).normalized;
        transform.Translate(Vector3.forward * webSpitSpeed * Time.deltaTime);
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
