using UnityEngine;

public class TreeFallScript : MonoBehaviour
{
    public GameObject hiddenBox;
    private Animator animator;
    private Animator animator2;
    private Animator animator3;
    private int treefellcount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hiddenBox.SetActive(false);
        animator = GetComponent<Animator>();
        animator2 = GetComponent<Animator>();
        animator3 = GetComponent<Animator>();
    }
    public void FirstTreeFall()
    {
        hiddenBox.SetActive(true);
        animator.SetTrigger("TreeFall");
    }
    public void SecondTreeFall()
    {
        animator.SetTrigger("Treefell1");
        treefellcount += 1;
    }
    public void ThridTreeFall()
    {
        animator.SetTrigger("TreeFell2");
    }
    public void SnowFall()
    {
        animator.SetTrigger("SnowFell");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstTreeFall();
        }
        if (other.CompareTag("archer"))
        {
            SecondTreeFall();
            if (treefellcount >= 1)
            {
                ThridTreeFall();
            }

            //TreeFell2
        }
        if (other.CompareTag("Arrows"))
        {
            SnowFall();
        }
        //hiddenBox.SetActive(true);
        //animator.SetTrigger("TreeFall");
    }
}
