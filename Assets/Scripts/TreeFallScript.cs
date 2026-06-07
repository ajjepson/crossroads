using UnityEngine;

public class TreeFallScript : MonoBehaviour
{
    public GameObject hiddenBox;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hiddenBox.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        hiddenBox.SetActive(true);
        animator.SetTrigger("TreeFall");
    }
}
