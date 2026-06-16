using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManagerCS : MonoBehaviour
{
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
}
