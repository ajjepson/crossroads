using UnityEngine;

public class ArrowsDamage : MonoBehaviour
{
    public static int ropecut1 = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            ropecut1 += 1;
            Debug.Log(ropecut1);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("BreakObject"))
        {
            Destroy(other.gameObject);
            Debug.Log("Object Destroyed");
        }
        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
