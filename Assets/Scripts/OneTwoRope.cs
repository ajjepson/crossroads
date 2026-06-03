using UnityEngine;

public class OneTwoRope : MonoBehaviour
{
    private Animator animator;
    public FireArrows FireArrows;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FireArrows.ropecut == 2)
        {
            animator.SetBool("BridgeFall", true);
        }
    }
}
