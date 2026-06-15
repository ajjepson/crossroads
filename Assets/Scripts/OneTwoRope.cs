using UnityEngine;

public class OneTwoRope : MonoBehaviour
{
    private Animator animator;
    public ArrowsDamage ArrowsDamage;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //from ArrowsDamage script
        if (ArrowsDamage.ropecut1 == 1)
        {
            animator.SetBool("BridgeFall", true);
        }
    }
}
