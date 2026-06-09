using System.Collections;
using UnityEngine;

public class IceShooter : MonoBehaviour
{
    [SerializeField]
    public GameObject iceattack1;
    public GameObject iceattack2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 Ice1Direction = new Vector3(12, 0, 0);
    Vector3 Ice2Direction = new Vector3(-12, 0, 0);
    void Start()
    {
        StartCoroutine(iceShooter1());
        StartCoroutine(iceShooter2());
        GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator iceShooter1()
    {
        Vector3 iceSpawnPosition = Vector3.zero;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            iceSpawnPosition.x = -171.38f;
            iceSpawnPosition.y = 1.1f;
            iceSpawnPosition.z = 155.4486f;
            GameObject icy1 = Instantiate(iceattack1, iceSpawnPosition, Quaternion.identity);
            icy1.GetComponent<Rigidbody>().AddForce(Ice1Direction, ForceMode.Impulse);
            Destroy(icy1, 5);

        }

    }
    IEnumerator iceShooter2()
    {
        Vector3 ice2SpawnPosition = Vector3.zero;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            ice2SpawnPosition.x = -129.67f;
            ice2SpawnPosition.y = 0.57f;
            ice2SpawnPosition.z = 174.76f;
            GameObject icy2 = Instantiate(iceattack2, ice2SpawnPosition, Quaternion.identity);
            icy2.GetComponent<Rigidbody>().AddForce(Ice2Direction, ForceMode.Impulse);
            Destroy(icy2, 5);

        }
    }
}
