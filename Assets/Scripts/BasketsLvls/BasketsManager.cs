using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketsManager : MonoBehaviour
{
    public AudioSource audioSource;     // Источник звука
    public AudioClip clickSound;        // Аудиофайл (звук клика)
    public GameObject music;

    public void MainMenu()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("MainMenu");
        music.SetActive(false);
    }

}
