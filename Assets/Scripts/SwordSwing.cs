using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private Animator animator;
    public bool attacking;
    public int damage = 10;
    private bool hasHitEnemyThisSwing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(attacking);
        //left click
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("SwingSword", true);
            attacking = true;
            hasHitEnemyThisSwing = false; 
            Invoke("SwingDelay", 2);
        }
    }
    void SwingDelay()
    {
        animator.SetBool("SwingSword", false);
        attacking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!attacking) return;
        if (other.CompareTag("enemy") && !hasHitEnemyThisSwing)
        {
            Debug.Log("You hit an Enemy");

            SkeletonController enemy = other.GetComponent<SkeletonController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                hasHitEnemyThisSwing = true;
            }
        }

        if (other.CompareTag("BreakObject") && attacking == true)
        {
            Destroy(other.gameObject);
            Debug.Log("Object Destroyed");
        }
    }

}

