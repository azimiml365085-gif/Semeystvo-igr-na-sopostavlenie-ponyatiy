using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Лог сообщений")]
    public Text messageLog; //тут будут сообщения
    public GameObject messageLogObj; //для активации

    [Header("Меню создания уровня")]
    public TMP_InputField fileNameInput;   // Поле для имени файла

    [Header("Меню вопросов")]
    public TMP_InputField questionsInput;  // Поле для вопросов

    [Header("Настройки")]
    public string baseFolder = "UserLevels/CosmicStranger"; // Базовая папка

    [Header("Связь с QuestionManager")]
    public QuestionManager questionManager;

    public AudioSource audioSource;     // Источник звука
    public AudioClip clickSound;        // Аудиофайл (звук клика)

    public static string currentLevelName = "";   // Имя текущего уровня
    public static string currentLevelPath = "";   // Полный путь к папке уровня

    // 1. Создать папку уровня (кнопка "ОК")
    public void CreateLevel()
    {
        audioSource.PlayOneShot(clickSound);

        // Проверяем имя
        if (string.IsNullOrWhiteSpace(fileNameInput.text))
        {
            Debug.LogWarning("Введите название уровня!");
            messageLogObj.SetActive(true);
            messageLog.text = "Введите название уровня!";
            return; 
        }

        // Очищаем имя от недопустимых символов
        string cleanName = fileNameInput.text;
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            cleanName = cleanName.Replace(c.ToString(), "");
        }

        if (string.IsNullOrEmpty(cleanName))
        {
            Debug.LogWarning("Имя содержит недопустимые символы!");
            messageLogObj.SetActive(true);
            messageLog.text ="Имя содержит недопустимые символы!";
            return;
        }

        currentLevelName = cleanName;

        // Путь к папке уровня
        string folderPath = Application.dataPath + "/" + baseFolder + "/" + currentLevelName;
        currentLevelPath = folderPath;

        // Проверяем, существует ли уже такая папка
        if (Directory.Exists(folderPath))
        {
            Debug.LogWarning("Уровень с именем '" + currentLevelName + "' уже существует!");
            messageLogObj.SetActive(true);
            messageLog.text ="Уровень с именем '" + currentLevelName + "' уже существует! Дальнейшие действия будут в нём";
            return;
        }

        // Создаём папку уровня
        Directory.CreateDirectory(folderPath);
        Debug.Log("Папка уровня создана: " + folderPath);
        messageLogObj.SetActive(true);
        messageLog.text ="Папка уровня создана: " + folderPath;

        // Создаём пустой файл вопросов
        string questionsPath = folderPath + "/Questions.txt";
        File.WriteAllText(questionsPath, "");
        Debug.Log("Файл вопросов создан: " + questionsPath);
        messageLogObj.SetActive(true);
        messageLog.text ="Файл вопросов создан: " + questionsPath;

        // Очищаем поле имени (опционально)
        // fileNameInput.text = "";

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // 2. Сохранить вопросы (кнопка "Сохранить")
    public void SaveQuestions()
    {
        audioSource.PlayOneShot(clickSound);

        // Проверяем, что уровень был создан
        if (string.IsNullOrEmpty(currentLevelPath))
        {
            Debug.LogWarning("Сначала создайте уровень! (Нажмите 'ОК' в меню создания)");
            messageLogObj.SetActive(true);
            messageLog.text ="Сначала создайте уровень! (Нажмите 'ОК' в меню создания)";
            return;
        }

        // Проверяем, что папка существует
        if (!Directory.Exists(currentLevelPath))
        {
            Debug.LogWarning("Папка уровня не найдена: " + currentLevelPath);
            messageLogObj.SetActive(true);
            messageLog.text ="Папка уровня не найдена: " + currentLevelPath;
            currentLevelPath = "";
            return;
        }

        // Проверяем, что есть текст
        if (string.IsNullOrWhiteSpace(questionsInput.text))
        {
            Debug.LogWarning("Нет вопросов для сохранения!");
            messageLogObj.SetActive(true);
            messageLog.text ="Нет вопросов для сохранения!";
            return;
        }

        // Путь к файлу вопросов
        string questionsPath = currentLevelPath + "/Questions.txt";

        // Сохраняем вопросы
        File.WriteAllText(questionsPath, questionsInput.text);
        Debug.Log("Вопросы сохранены в: " + questionsPath);
        messageLogObj.SetActive(true);
        messageLog.text ="Вопросы сохранены в: " + questionsPath;

        // Автоматически загружаем вопросы в QuestionManager
        if (questionManager != null)
        {
            questionManager.LoadQuestionsFromFile(currentLevelName);
            Debug.Log("Вопросы автоматически загружены в QuestionManager!");
            messageLogObj.SetActive(true);
            messageLog.text ="Вопросы автоматически загружены на панель вопросов!";
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // 3. Загрузить уровень (для кнопки выбора)
    public void LoadLevel(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Имя уровня пустое!");
            messageLogObj.SetActive(true);
            messageLog.text ="Имя уровня пустое!";
            return;
        }

        currentLevelName = levelName;
        currentLevelPath = Application.dataPath + "/" + baseFolder + "/" + levelName;

        if (!Directory.Exists(currentLevelPath))
        {
            Debug.LogWarning("Папка уровня не найдена: " + currentLevelPath);
            messageLogObj.SetActive(true);
            messageLog.text ="Папка уровня не найдена: " + currentLevelPath;
            return;
        }

        // Загружаем вопросы
        if (questionManager != null)
        {
            questionManager.LoadQuestionsFromFile(levelName);
        }

        Debug.Log("Уровень загружен: " + levelName);
        messageLogObj.SetActive(true);
        messageLog.text ="Уровень загружен: " + levelName;
    }

    // 4. Получить список всех уровней
    public string[] GetLevelList()
    {
        string folderPath = Application.dataPath + "/" + baseFolder;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            return new string[0];
        }

        // Получаем все папки (уровни)
        string[] directories = Directory.GetDirectories(folderPath);

        // Извлекаем только имена папок
        for (int i = 0; i < directories.Length; i++)
        {
            directories[i] = Path.GetFileName(directories[i]);
        }

        return directories;
    }

    // 5. Сброс
    public void ResetLevel()
    {
        currentLevelName = "";
        currentLevelPath = "";
        if (fileNameInput != null) fileNameInput.text = "";
        if (questionsInput != null) questionsInput.text = "";
        if (questionManager != null) questionManager.ResetQuestions();
        Debug.Log("Сброшено");
        messageLogObj.SetActive(true);
        messageLog.text ="Сброшено";
    }
}