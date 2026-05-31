using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HowToPlay : MonoBehaviour
{
    public TMP_Text howToPlayText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        howToPlayText.text = "How to Play: Use WASD or The arrow Keys To move";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Help1"))
        {
            howToPlayText.text = "How to Play: Use WASD or The arrow Keys To move";
        }
        else if (other.CompareTag("Help2"))
        {
            howToPlayText.text = "as the knight use left mouse button to swing sword";
        }
        else
        {
            howToPlayText.text = "";
        }
    }
}
