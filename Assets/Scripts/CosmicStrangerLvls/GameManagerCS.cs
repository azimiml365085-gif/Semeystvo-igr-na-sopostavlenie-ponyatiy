using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerCS : MonoBehaviour
{
    public static int score = 0;
    public Text scoreText;
    public AudioSource audioSource;     // Источник звука
    public AudioClip clickSound;        // Аудиофайл (звук клика)
    public GameObject menuPanel;
    public void Awake()
    {
        DontDestroyOnLoad(audioSource);
    }

    public void Start()
    {
        menuPanel.SetActive(false);
    }

    public void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideMenuPanel()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void Update()
    {
        scoreText.text = "Счёт: " + score.ToString();
    }
}
