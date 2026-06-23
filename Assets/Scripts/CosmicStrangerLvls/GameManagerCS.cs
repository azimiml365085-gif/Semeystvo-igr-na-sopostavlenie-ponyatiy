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
    public void Awake()
    {
        DontDestroyOnLoad(audioSource);
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
