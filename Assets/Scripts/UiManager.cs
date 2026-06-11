using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("1-1 Village");
    }
    public void HelpButton()
    {
        // Changed it to Isaiah Test SceneManager.LoadScene("HelpScreen");
        SceneManager.LoadScene("IsaiahTest");
    }
    public void MenuButton()
    {
        SceneManager.LoadScene("MenuScreen");
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit ButtonPressed");
    }
}
