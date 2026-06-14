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
    public Transform firePoint;
    public Image arrowsImage;
    //new

    //audio
    [SerializeField] private AudioClip bowShotAudio;
    private AudioSource audioBowSource;
    //
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

        audioBowSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        amount.text = fireCount.ToString();
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1 key
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
            //fire arrow is selected
            amount.enabled = true;
            Debug.Log("fire");
            typeOfArrow.text = "Fire";
            arrowImage.sprite = fireSprite;
            arrowFire = true;
            arrowIce = false;
            arrowNormal = false;
        }
        if (Input.GetMouseButtonDown(1) && canPlayerShoot == true)
        {
            //arrows audio

            audioBowSource.clip = bowShotAudio;
            audioBowSource.Play();
            StartCoroutine(ArrowsAttack());
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            Vector3 direction = (hit.point - firePoint.position).normalized;

            Rigidbody arrowToShoot;
            float speed;

            if (arrowFire)
            {
                if (fireCount <= 0)
                {
                    Debug.Log("Out of fire arrows!");
                    return;
                }
                arrowToShoot = fireArrow;
                speed = fireSpeed;
                fireCount--;
            }
            else if (arrowIce)
            {
                arrowToShoot = iceArrow;
                speed = iceSpeed;
            }
            else
            {
                arrowToShoot = arrow;
                speed = normalSpeed;
            }
            Rigidbody projectile = Instantiate(
               arrowToShoot,
               firePoint.position,
               Quaternion.identity
           );
            projectile.transform.forward = direction;
            projectile.linearVelocity = direction * speed;
            Destroy(projectile.gameObject, 5f);


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
}
