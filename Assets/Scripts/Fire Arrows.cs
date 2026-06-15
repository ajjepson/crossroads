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
    private float iceSpeed = 5f;

    private int fireCount = 30;

    public bool arrowFire;
    public bool arrowIce;
    public bool arrowNormal;

    public TMP_Text typeOfArrow;
    public TMP_Text amount;

    public Image arrowImage;
    public Sprite fireSprite;
    public Sprite iceSprite;
    public Sprite normalSprite;

    public int ropecut = 0;

    public float arrowsCoolDown = 1f;
    public bool canPlayerShoot = true;
    public bool arrowsHitbox = false;

    public Transform firePoint;
    public Image arrowsImage;

    [SerializeField] private AudioClip bowShotAudio;
    private AudioSource audioBowSource;

    void Start()
    {
        amount.text = fireCount.ToString();
        amount.enabled = false;

        typeOfArrow.text = "Normal";

        arrowFire = false;
        arrowIce = false;
        arrowNormal = true;

        arrowImage.sprite = normalSprite;

        audioBowSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        amount.text = fireCount.ToString();

        // Arrow switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            amount.enabled = false;
            arrowImage.sprite = normalSprite;
            typeOfArrow.text = "Normal";

            arrowFire = false;
            arrowIce = false;
            arrowNormal = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            amount.enabled = false;
            arrowImage.sprite = iceSprite;
            typeOfArrow.text = "Ice";

            arrowFire = false;
            arrowIce = true;
            arrowNormal = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            amount.enabled = true;
            arrowImage.sprite = fireSprite;
            typeOfArrow.text = "Fire";

            arrowFire = true;
            arrowIce = false;
            arrowNormal = false;
        }

        // Shooting
        if (Input.GetMouseButtonDown(1) && canPlayerShoot)
        {
            audioBowSource.clip = bowShotAudio;
            audioBowSource.Play();

            StartCoroutine(ArrowsAttack());

           
            Plane plane = new Plane(Vector3.up, firePoint.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!plane.Raycast(ray, out float distance))
                return;

            Vector3 targetPoint = ray.GetPoint(distance);

            Vector3 direction =
                (targetPoint - firePoint.position).normalized;

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
