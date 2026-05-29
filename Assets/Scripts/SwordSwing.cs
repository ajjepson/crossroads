using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private Animator animator;
    public bool attacking;
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
        if (attacking == true)
        {
            if(other.CompareTag("enemy"))
            {
                //emeny take damage
                Debug.Log("enemt took damage");
            }
        }
    }
}
