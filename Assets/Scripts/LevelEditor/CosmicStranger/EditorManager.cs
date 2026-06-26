using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public AudioSource audioSource;     // Источник звука
    public AudioClip clickSound;        // Аудиофайл (звук клика)
    public GameObject editorPanel; //Меню редактора
    public GameObject questionPanel; //Меню вопросов

    private void Start()
    {
        questionPanel.SetActive(false);
    }

    public void HideEditorMenu()
    {
        audioSource.PlayOneShot(clickSound);
        editorPanel.SetActive(false);
    }

    public void ShowEditorMenu()
    {
        audioSource.PlayOneShot(clickSound);
        editorPanel.SetActive(true);
    }

    public void ShowQuestionMenu()
    {
        audioSource.PlayOneShot(clickSound);
        editorPanel.SetActive(false);
        questionPanel.SetActive(true);
    }

    public void HideQuestionMenu()
    {
        audioSource.PlayOneShot(clickSound);
        questionPanel.SetActive(false);
        editorPanel.SetActive(true);
    }


}
