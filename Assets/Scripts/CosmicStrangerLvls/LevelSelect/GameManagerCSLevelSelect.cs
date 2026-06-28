using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerCSLevelSelect : MonoBehaviour
{
    [Header("Панели")]
    public GameObject cSharpPanel;
    public GameObject pascalPanel;
    public GameObject pythonPanel;
    public GameObject userPanel;



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

    public void HideCSharpPanel()
    {
        audioSource.PlayOneShot(clickSound);
        cSharpPanel.SetActive(false);
    }

    public void HidePascalPanel()
    {
        audioSource.PlayOneShot(clickSound);
        pascalPanel.SetActive(false);
    }

    public void HidePythonPanel()
    {
        audioSource.PlayOneShot(clickSound);
        pythonPanel.SetActive(false);
    }

    public void HideUserPanel()
    {
        audioSource.PlayOneShot(clickSound);
        userPanel.SetActive(false);
    }

    public void ShowCSharpPanel()
    {
        audioSource.PlayOneShot(clickSound);
        cSharpPanel.SetActive(true);
    }

    public void ShowPascalPanel()
    {
        audioSource.PlayOneShot(clickSound);
        pascalPanel.SetActive(true);
    }

    public void ShowPythonPanel()
    {
        audioSource.PlayOneShot(clickSound);
        pythonPanel.SetActive(true);
    }

    public void ShowUserPanel()
    {
        audioSource.PlayOneShot(clickSound);
        userPanel.SetActive(true);
    }

    //C# уровни
    public void CSharpLevel1()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Массивы");
    }

    public void CSharpLevel2()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Методы и функции");
    }

    public void CSharpLevel3()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("ООП");
    }

    public void CSharpLevel4()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Типы данных");
    }


    //Pascal уровни
    public void PascalLevel1()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Массивы(Паскаль)");
    }

    //Python уровни
    public void PythonLevel1()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Списки(Питон)");
    }



}