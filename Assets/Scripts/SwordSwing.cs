using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SwordSwing : MonoBehaviour
{
    private Animator animator;
    public bool attacking;
    public int damage = 10;
    private bool hasHitEnemyThisSwing;
    //new
    public float swingCoolDown = 0.5f;
    public bool canPlayerSwing = true;
    public bool swingHitbox = false;

    public Image swordImage;

    public FinalBoss FinalBoss;
    public SpiderBoss SpiderBoss;
    public Transform swordPivot;
    //audio
    [SerializeField] private AudioClip swordAtackAudio;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //attacking = false;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(attacking);
        //left click
        AimAtMouse();

        if (Input.GetMouseButtonDown(0) && canPlayerSwing)
        {
            StartCoroutine(SwordAttack());
        }

        //New got rid 0f animator.SetBool("SwingSword", true);
        //New got rid 0f attacking = true;
        //New got rid 0f hasHitEnemyThisSwing = false;
        //New got rid of Invoke("SwingDelay", 2);
        //new

    }
    void AimAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 target = hit.point;
            target.y = swordPivot.position.y;

            swordPivot.LookAt(target);
        }
    }
    void SwingDelay()
    {
        animator.SetBool("SwingSword", false);
        attacking = false;
    }
    private IEnumerator SwordAttack()
    {
        canPlayerSwing = false;
        swingHitbox = true;
        animator.SetTrigger("SwordTrigger");
        //play audio
        audioSource.clip = swordAtackAudio;
        audioSource.Play();
        //animator.SetBool("SwingSword", true);
        swordImage.fillAmount = 0f;
        float countdown = 0f;
        while (countdown < swingCoolDown)
        {
            countdown += Time.deltaTime;
            swordImage.fillAmount = (countdown / swingCoolDown);
            yield return null;

        }
        swordImage.fillAmount = 1f;
        swingHitbox = false;
        //animator.SetBool("SwingSword", false);
        canPlayerSwing = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //New got rid 0f (!attacking) return;
        /*
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
        */
        if (other.CompareTag("enemy") && /*replaced canPlayerSwing*/ swingHitbox == true )
        {
            Debug.Log("You hit an Enemy");

            SkeletonController enemy = other.GetComponent<SkeletonController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                //hasHitEnemyThisSwing = true;
            }
        }

        if (other.CompareTag("spider") && swingHitbox == true)
        {
            Debug.Log("You hit an spider");
            if (SpiderBoss != null)
            SpiderBoss.spiderHealth -= 10;
        }
        if (other.CompareTag("Boss") && swingHitbox == true)
        {
            Debug.Log("You hit an boss");
            if (FinalBoss != null)
                FinalBoss.finalHealth -= 10;
        }

        if (other.CompareTag("BreakObject") && swingHitbox == true)
        {
            Destroy(other.gameObject);
            Debug.Log("Object Destroyed");
        }
    }

}

