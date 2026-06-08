using UnityEngine;

public class OneTwoRope : MonoBehaviour
{
    private Animator animator;
    public ArrowsDamage ArrowsDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ArrowsDamage.ropecut1 == 1)
        {
            animator.SetBool("BridgeFall", true);
        }
    }
}
