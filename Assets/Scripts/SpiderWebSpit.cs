using Unity.AppUI.UI;
using UnityEngine;

public class SpiderWebSpit : MonoBehaviour
{
    public float webSpitSpeed = 15;
    private Rigidbody rigidbod;
    private Transform chasePlayer;
    public float chasePlayerCount = 1;
    private Vector3 afterChaseEnd;
    private float chaseTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbod = GetComponent<Rigidbody>();
        //rigidbod.angularVelocity = transform.forward * webSpitSpeed;
        chasePlayer = GameObject.FindGameObjectWithTag("Player").transform;
        chaseTimer = chasePlayerCount;
        Destroy(gameObject, 7f);


    }
    private void Update()
    {
        if (chasePlayer == null)
        {
            //player cant be found
            return;
        }

        if (chaseTimer > 0)
        {
            chaseTimer -= Time.deltaTime;
            Vector3 playerDirection = (chasePlayer.position - transform.position).normalized;
            transform.position += playerDirection * webSpitSpeed * Time.deltaTime;
            afterChaseEnd = playerDirection;
        }
        else
        {
            transform.position += afterChaseEnd * webSpitSpeed * Time.deltaTime;
        }
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
