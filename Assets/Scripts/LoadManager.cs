using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("UI")]
    public Text progressText;  // Остался только текст

    [Header("Настройки")]
    public string sceneToLoad = "MainMenu";

    private bool isLoadingComplete = false;
    private AsyncOperation operation;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    void Update()
    {
        // Если загрузка завершена И нажата любая кнопка
        if (isLoadingComplete && Input.anyKeyDown)
        {
            // Разрешаем переход
            operation.allowSceneActivation = true;
        }
    }

    IEnumerator LoadSceneAsync()
    {
        // Начинаем загрузку, но запрещаем автоматический переход
        operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        // Пока загрузка не дошла до 0.9 (это 100% в Unity)
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Обновляем только текст
            if (progressText != null)
                progressText.text = "Загрузка... " + (progress * 100).ToString("0") + "%";

            yield return null;
        }

        // Когда загрузка завершена (progress >= 0.9)
        isLoadingComplete = true;

        // Меняем текст
        if (progressText != null)
            progressText.text = "Нажми любую кнопку, чтобы начать!";

        // Ждём, пока игрок нажмёт кнопку (Update сделает это)
        while (!isLoadingComplete)
        {
            yield return null;
        }
    }
}