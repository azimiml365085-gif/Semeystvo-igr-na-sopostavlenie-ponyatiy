using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject quitMenu;
    public AudioSource audioSource;     // Источник звука
    public AudioClip clickSound;        // Аудиофайл (звук клика)

    public void Awake()
    {
        DontDestroyOnLoad(audioSource);
        quitMenu.SetActive(false);
    }

    public void Cosmic()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("CosmicStrangerBase");
    }

    public void Baskets()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("BasketsBase");
    }

    public void MainMenu()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void Help()
    {
        audioSource.PlayOneShot(clickSound);
        Application.OpenURL("file:///" + Application.streamingAssetsPath + "/Help.chm");
    }


    public void Exit()
    {
        audioSource.PlayOneShot(clickSound);
        quitMenu.SetActive(true);
    }

    public void QuitNo()
    {
        audioSource.PlayOneShot(clickSound);
        quitMenu.SetActive(false);
    }

    public void QuitYes()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }
}
