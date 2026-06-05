using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FireArrows : MonoBehaviour
{
    public Rigidbody fireArrow;
    public Rigidbody iceArrow;
    public Rigidbody arrow;
    private float normalSpeed = 7.5f;
    private float fireSpeed = 10f;
    private int fireCount = 30;
    private float iceSpeed = 5f;
    public bool arrowFire;
    public bool arrowIce;
    public bool arrowNormal;
    public TMP_Text typeOfArrow;
    public TMP_Text amount;

    public Image arrowImage;
    public Sprite fireSprite;
    public Sprite iceSprite;
    public Sprite normalSprite;
    //for level 1-2
    public int ropecut = 0;
    //
    //new
    public float arrowsCoolDown = 1f;
    public bool canPlayerShoot = true;
    public bool arrowsHitbox = false;

    public Image arrowsImage;
    //new
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amount.text = fireCount.ToString();
        amount.enabled = false;
        typeOfArrow.text = "Normal";
        arrowFire = false;
        arrowIce = false;
        arrowNormal = true;
        //arrowImage = GetComponent<Image>();
        arrowImage.sprite = normalSprite;
    }

    // Update is called once per frame
    void Update()
    {
        amount.text = fireCount.ToString();
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1 key
        {
            //fire arrow is selected
            amount.enabled = true;
            Debug.Log("fire");
            typeOfArrow.text = "Fire";
            arrowImage.sprite = fireSprite;
            arrowFire = true;
            arrowIce = false;
            arrowNormal = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //2 key
        {
            //ice arrow is selected
            Debug.Log("ice");
            amount.enabled = false;
            arrowImage.sprite = iceSprite;
            typeOfArrow.text = "Ice";
            arrowFire = false;
            arrowIce = true;
            arrowNormal = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) //3 key
        {
            //normal arrow is selected
            amount.enabled = false;
            Debug.Log("normal");
            arrowImage.sprite = normalSprite;
            typeOfArrow.text = "Normal";
            arrowFire = false;
            arrowIce = false;
            arrowNormal = true;
        }
        if (Input.GetMouseButtonDown(1) && canPlayerShoot == true)
        {
            StartCoroutine(ArrowsAttack());

            if (Input.GetMouseButtonDown(1) && arrowFire == true)
            {
                //New
                //StartCoroutine(ArrowsAttack());
                //New
                if (fireCount <= 0)
                {
                    //cant fire
                    Debug.Log("bullets:" + fireCount);
                }
                else
                {
                    Rigidbody fire = Instantiate(fireArrow, transform.position, transform.rotation);
                    fire.linearVelocity = transform.forward * fireSpeed;
                    Destroy(fire.gameObject, 5f);
                    fireCount -= 1;
                    Debug.Log("bullets:" + fireCount);
                }
            }
            else if (Input.GetMouseButtonDown(1) && arrowIce == true)
            {
                //New
                //StartCoroutine(ArrowsAttack());
                //New

                Rigidbody ice = Instantiate(iceArrow, transform.position, transform.rotation);
                ice.linearVelocity = transform.forward * iceSpeed;
                Destroy(ice.gameObject, 5f);
            }
            else if (Input.GetMouseButtonDown(1) && arrowNormal == true)
            {
                //New
                //StartCoroutine(ArrowsAttack());
                //New

                Rigidbody normal = Instantiate(arrow, transform.position, transform.rotation);
                normal.linearVelocity = transform.forward * normalSpeed;
                Destroy(normal.gameObject, 5f);
            }
            else
            {
                //no arrow was fired
            }
        }
    }
    private IEnumerator ArrowsAttack()
    {
        canPlayerShoot = false;
        arrowsHitbox = true;
        arrowsImage.fillAmount = 0f;
        float arrowCountdown = 0f;
        while (arrowCountdown < arrowsCoolDown)
        {
            arrowCountdown += Time.deltaTime;
            arrowsImage.fillAmount = (arrowCountdown / arrowsCoolDown);
            yield return null;

        }
        arrowsImage.fillAmount = 1f;
        arrowsHitbox = false;
        canPlayerShoot = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rope"))
        {
            ropecut += 1;
            Debug.Log(ropecut);
            Destroy(other.gameObject);
        }
    }
}
