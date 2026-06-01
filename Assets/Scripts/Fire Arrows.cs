using UnityEngine;

public class FireArrows : MonoBehaviour
{
    public Rigidbody fireArrow;
    public Rigidbody iceArrow;
    public Rigidbody arrow;
    private float normalSpeed = 5;
    private float fireSpeed = 7.5f;
    private float iceSpeed = 2.5f;
    public bool arrowFire;
    public bool arrowIce;
    public bool arrowNormal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrowFire = false;
        arrowIce = false;
        arrowNormal = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1 key
        {
            //fire arrow is selected
            Debug.Log("fire");
            arrowFire = true;
            arrowIce = false;
            arrowNormal = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //2 key
        {
            //ice arrow is selected
            Debug.Log("ice");
            arrowFire = false;
            arrowIce = true;
            arrowNormal = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) //3 key
        {
            //normal arrow is selected
            Debug.Log("normal");
            arrowFire = false;
            arrowIce = false;
            arrowNormal = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && arrowFire == true)
        {
            Rigidbody fire = Instantiate(fireArrow, transform.position, transform.rotation);
            fire.angularVelocity = transform.forward * fireSpeed;
            Destroy( fire.gameObject, 5f );
        }
        else if (Input.GetKeyDown(KeyCode.Space) && arrowIce == true)
        {
            Rigidbody ice = Instantiate(iceArrow, transform.position, transform.rotation);
            ice.angularVelocity = transform.forward * iceSpeed;
            Destroy(ice.gameObject, 5f);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && arrowNormal == true)
        {
            Rigidbody normal = Instantiate(arrow, transform.position, transform.rotation);
            normal.angularVelocity = transform.forward * normalSpeed;
            Destroy(normal.gameObject, 5f);
        }
        else
        {
            //no arrow was fired
        }
    }
}
