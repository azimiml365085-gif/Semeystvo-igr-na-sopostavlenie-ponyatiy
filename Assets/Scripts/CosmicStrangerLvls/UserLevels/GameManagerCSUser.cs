using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerCSUser : MonoBehaviour
{
    public static int score = 0;
    public Text scoreText;
    public AudioSource audioSource;
    public AudioClip clickSound;
    public GameObject menuPanel;

    void Awake()
    {
        DontDestroyOnLoad(audioSource);
    }

    void Start()
    {
        menuPanel.SetActive(false);
        // Сбрасываем счет при старте
        score = 0;
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
        score = 0; // Сбрасываем счет при выходе в меню
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (scoreText != null)
            scoreText.text = "Счёт: " + score.ToString();
    }
}