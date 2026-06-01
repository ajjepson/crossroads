using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private Animator animator;
    public bool attacking;
    public int damage = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacking = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //left click
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("SwingSword", true);
            attacking = true;
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
      if (attacking && other.CompareTag("Enemy"))
        {
            Debug.Log("you hit an Enemy");
            //deal damage to enemy
            SkeletonController enemy = other.GetComponent<SkeletonController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if (other.CompareTag("BreakObject"))
            {
                Destroy(other.gameObject);
                Debug.Log("Object Destroyed Object");
            }
        }
    }
}
