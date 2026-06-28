using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
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

    public void Cosmic()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("CosmicStrangerLevelEditor");
    }

    public void Baskets()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("BasketsBase");
    }


}
