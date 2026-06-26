using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class QuestionManager : MonoBehaviour
{
    [Header("UI")]
    public Text legacyText;

    [Header("Настройки")]
    public string baseFolder = "UserLevels/CosmicStranger";

    private string[] lines;
    private int index = 0;
    private static QuestionManager instance;
    private string currentLevelName = "";

    public static QuestionManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

    // Загрузить вопросы из папки уровня
    public void LoadQuestionsFromFile(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Имя уровня пустое!");
            return;
        }

        currentLevelName = levelName;

        // Путь к файлу вопросов
        string folderPath = Application.dataPath + "/" + baseFolder + "/" + levelName;
        string filePath = folderPath + "/Questions.txt";

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Файл вопросов не найден: " + filePath);
            legacyText.text = "Файл не найден!";
            return;
        }

        // Читаем файл
        string fileContent = File.ReadAllText(filePath);
        lines = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            legacyText.text = "Нет вопросов!";
            return;
        }

        index = 0;
        UpdateText();
        Debug.Log("Загружено " + lines.Length + " вопросов из уровня: " + levelName);
    }

    // Статический метод для смены текста
    public static void ChangeText()
    {
        if (instance != null)
        {
            instance.NextLine();
        }
        else
        {
            Debug.LogWarning("QuestionManager не существует на сцене!");
        }
    }

    // Переход к следующему вопросу
    public void NextLine()
    {
        if (lines != null && lines.Length > 0)
        {
            index = (index + 1) % lines.Length;
            UpdateText();
        }
        else
        {
            legacyText.text = "Нет вопросов для отображения!";
        }
    }

    // Обновить текст на экране
    void UpdateText()
    {
        if (legacyText != null && lines != null && lines.Length > 0)
        {
            legacyText.text = lines[index];
        }
    }

    // Получить текущий вопрос
    public string GetCurrentQuestion()
    {
        if (lines != null && lines.Length > 0)
        {
            return lines[index];
        }
        return "";
    }

    // Сброс
    public void ResetQuestions()
    {
        lines = null;
        index = 0;
        currentLevelName = "";
        legacyText.text = "Загрузите уровень";
    }
}