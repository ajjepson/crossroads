using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireArrows : MonoBehaviour
{
    public Rigidbody fireArrow;
    public Rigidbody iceArrow;
    public Rigidbody arrow;
    private float normalSpeed = 7.5f;
    private float fireSpeed = 10f;
    private float iceSpeed = 5f;
    public bool arrowFire;
    public bool arrowIce;
    public bool arrowNormal;
    public TMP_Text typeOfArrow;

    public Image arrowImage;
    public Sprite fireSprite;
    public Sprite iceSprite;
    public Sprite normalSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1 key
        {
            //fire arrow is selected
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
            arrowImage.sprite = iceSprite;
            typeOfArrow.text = "Ice";
            arrowFire = false;
            arrowIce = true;
            arrowNormal = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) //3 key
        {
            //normal arrow is selected
            Debug.Log("normal");
            arrowImage.sprite = normalSprite;
            typeOfArrow.text = "Normal";
            arrowFire = false;
            arrowIce = false;
            arrowNormal = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && arrowFire == true)
        {
            Rigidbody fire = Instantiate(fireArrow, transform.position, transform.rotation);
            fire.linearVelocity = transform.forward * fireSpeed;
            Destroy( fire.gameObject, 5f );
        }
        else if (Input.GetKeyDown(KeyCode.Space) && arrowIce == true)
        {
            Rigidbody ice = Instantiate(iceArrow, transform.position, transform.rotation);
            ice.linearVelocity = transform.forward * iceSpeed;
            Destroy(ice.gameObject, 5f);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && arrowNormal == true)
        {
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
