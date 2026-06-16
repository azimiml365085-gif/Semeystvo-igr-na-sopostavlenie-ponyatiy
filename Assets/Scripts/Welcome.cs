using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [Header("Настройки")]
    public string nextSceneName = "LoadingScreen"; // Куда переходим
    public float minDisplayTime = 3f; // Минимальное время показа логотипа
    public float fadeDuration = 1f; // Время затухания/появления

    [Header("UI")]
    public Image blackPanel; // Чёрная панель (Image)

    private bool canProceed = false;
    private bool isFading = false;

    void Start()
    {
        // Начинаем с полностью чёрного экрана
        blackPanel.color = new Color(0, 0, 0, 1);

        // Запускаем последовательность
        StartCoroutine(SplashSequence());
    }

    void Update()
    {
        // Если нажали ЛКМ и можно переходить, и не идёт затухание
        if (Input.GetMouseButtonDown(0) && !isFading && canProceed)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator SplashSequence()
    {
        // 1. Ждём 0.5 секунды (чтобы игрок увидел чёрный экран)
        yield return new WaitForSeconds(0.5f);

        // 2. Плавное появление логотипа (чёрная панель становится прозрачной)
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            blackPanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackPanel.color = new Color(0, 0, 0, 0); // Полностью прозрачная

        // 3. Теперь можно нажимать кнопку
        canProceed = true;

        // 4. Если игрок не нажимает кнопку, переходим автоматически через minDisplayTime
        yield return new WaitForSeconds(minDisplayTime);

        // Если игрок уже нажал — не делаем ничего
        if (!isFading)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        isFading = true;
        canProceed = false;

        // 5. Затухание до чёрного экрана (панель становится непрозрачной)
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            blackPanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackPanel.color = new Color(0, 0, 0, 1); // Полностью чёрная

        // 6. Переход на следующую сцену
        SceneManager.LoadScene(nextSceneName);
    }
}