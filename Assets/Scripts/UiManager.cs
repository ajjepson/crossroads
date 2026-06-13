using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickAudio;
    private AudioSource audioButtonSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioButtonSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayButton()
    {
        audioButtonSource.clip = buttonClickAudio;
        audioButtonSource.Play();
        SceneManager.LoadScene("1-1 Village");
    }
    public void HelpButton()
    {
        audioButtonSource.clip = buttonClickAudio;
        audioButtonSource.Play();
        // Changed it to Isaiah Test SceneManager.LoadScene("HelpScreen");
        SceneManager.LoadScene("IsaiahTest");
    }
    public void MenuButton()
    {
        audioButtonSource.clip = buttonClickAudio;
        audioButtonSource.Play();
        SceneManager.LoadScene("MenuScreen");
    }
    public void CreditsButton()
    {
        audioButtonSource.clip = buttonClickAudio;
        audioButtonSource.Play();
        SceneManager.LoadScene("Credits");
    }
    public void QuitButton()
    {
        audioButtonSource.clip = buttonClickAudio;
        audioButtonSource.Play();
        Application.Quit();
        Debug.Log("Quit ButtonPressed");
    }
}
