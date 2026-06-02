using UnityEngine;
using System;
using UnityEngine.AI;

public class HealItem : MonoBehaviour
{
    public GameObject healthPickup;
    public SwordSwing SwordSwing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwordSwing.attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (SwordSwing.attacking == true)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                if (healthPickup != null)
                {
                    Instantiate(healthPickup, transform.position, Quaternion.identity);
                }
            }
        }
        Destroy(gameObject);
    }
}
