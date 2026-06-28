using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class GameDataLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text levelNameText;
    [SerializeField] private Text questionText;

    [Header("Objects")]
    [SerializeField] private Transform objectsContainer;
    [SerializeField] private GameObject answerPrefab;

    private string currentLevelPath;
    private List<GameObject> spawnedAnswers = new List<GameObject>();

    private string[] questions;
    private string[] answers;
    private string[] positions;
    private string[] isCorrect;
    private int currentQuestionIndex = 0;

    void Start()
    {
        currentLevelPath = LevelSelector.SelectedLevelPath;

        if (string.IsNullOrEmpty(currentLevelPath))
        {
            Debug.LogError("Путь к уровню не передан!");
            return;
        }

        LoadLevel(currentLevelPath);
    }

    public void LoadLevel(string path)
    {
        ClearLevel();

        LoadFiles(path);
        ShowQuestion(0);

        string levelName = Path.GetFileName(path);
        if (levelNameText != null)
            levelNameText.text = levelName;

        Debug.Log($"Загружен уровень: {levelName}");
        Debug.Log($"Вопросов: {questions?.Length ?? 0}");
        Debug.Log($"Ответов: {answers?.Length ?? 0}");
    }

    private void LoadFiles(string path)
    {
        string questionsPath = Path.Combine(path, "Questions.txt");
        string answersPath = Path.Combine(path, "Answers.txt");
        string positionsPath = Path.Combine(path, "AnswerPositions.txt");
        string isCorrectPath = Path.Combine(path, "AnswerIsCorrect.txt");

        if (!File.Exists(questionsPath) || !File.Exists(answersPath) ||
            !File.Exists(positionsPath) || !File.Exists(isCorrectPath))
        {
            Debug.LogError($"Не все файлы найдены в {path}");
            return;
        }

        questions = File.ReadAllLines(questionsPath);
        answers = File.ReadAllLines(answersPath);
        positions = File.ReadAllLines(positionsPath);
        isCorrect = File.ReadAllLines(isCorrectPath);

        if (answers.Length != positions.Length || answers.Length != isCorrect.Length)
        {
            Debug.LogError($"Количество строк не совпадает: Answers={answers.Length}, Positions={positions.Length}, IsCorrect={isCorrect.Length}");
            return;
        }
    }

    private void ShowQuestion(int index)
    {
        if (questions == null || index >= questions.Length)
        {
            Debug.LogError("Нет вопросов для отображения");
            return;
        }

        if (questionText != null)
        {
            questionText.text = questions[index];
        }

        ClearAnswers();

        int answersPerQuestion = 4;
        int startIndex = index * answersPerQuestion;
        int endIndex = Mathf.Min(startIndex + answersPerQuestion, answers.Length);

        for (int i = startIndex; i < endIndex; i++)
        {
            CreateAnswerObject(i);
        }
    }

    private void CreateAnswerObject(int index)
    {
        if (answerPrefab == null)
        {
            Debug.LogError("AnswerPrefab не назначен!");
            return;
        }

        // Спавним объект без родителя (в корень сцены)
        GameObject answerObj = Instantiate(answerPrefab);

        // Если есть контейнер - делаем его дочерним, но сохраняем мировые координаты
        if (objectsContainer != null)
        {
            answerObj.transform.SetParent(objectsContainer, true); // true = сохранять мировые координаты
        }

        // Устанавливаем позицию в мировых координатах
        Vector3 position = ParsePosition(positions[index]);
        answerObj.transform.position = position;

        // Проверяем Z координату
        Vector3 pos = answerObj.transform.position;
        if (pos.z != 0)
        {
            pos.z = 0;
            answerObj.transform.position = pos;
        }

        // Находим текст в дочерних объектах
        Text textComponent = answerObj.GetComponentInChildren<Text>();
        if (textComponent != null)
        {
            textComponent.text = answers[index];
            Debug.Log($"Установлен текст: {answers[index]}");
        }
        else
        {
            Debug.LogWarning($"У префаба нет компонента Text в дочерних объектах!");
        }

        // Настраиваем скрипт
        AnswerObject answerScript = answerObj.GetComponent<AnswerObject>();
        if (answerScript != null)
        {
            bool correct = isCorrect[index].Trim() == "1";
            answerScript.SetCorrect(correct);
            answerScript.SetIndex(index);
            answerScript.OnAnswerSelected += HandleAnswerSelected;

            Debug.Log($"Создан объект {index}: '{answers[index]}' на позиции {position}, правильный: {correct}");
        }
        else
        {
            Debug.LogWarning($"У префаба нет компонента AnswerObject!");
        }

        spawnedAnswers.Add(answerObj);
    }

    private Vector3 ParsePosition(string positionStr)
    {
        // Формат: "7,36,0,41,0,00" - берем первые 3 значения
        string[] parts = positionStr.Replace(" ", "").Split(',');

        if (parts.Length >= 3)
        {
            float x = float.Parse(parts[0]);
            float y = float.Parse(parts[1]);
            float z = float.Parse(parts[2]);
            return new Vector3(x, y, z);
        }

        Debug.LogWarning($"Не удалось распарсить позицию: {positionStr}");
        return Vector3.zero;
    }

    private void HandleAnswerSelected(bool isCorrect, int index)
    {
        if (isCorrect)
        {
            Debug.Log($"✅ Правильный ответ! Индекс: {index}");
            // Твоя логика победы
        }
        else
        {
            Debug.Log($"❌ Неправильный ответ! Индекс: {index}");
            // Твоя логика поражения
        }
    }

    private void ClearAnswers()
    {
        foreach (GameObject obj in spawnedAnswers)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedAnswers.Clear();
    }

    public void ClearLevel()
    {
        ClearAnswers();
        questions = null;
        answers = null;
        positions = null;
        isCorrect = null;
        currentQuestionIndex = 0;
    }

    public void NextQuestion()
    {
        if (questions != null && currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Это был последний вопрос!");
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            ShowQuestion(currentQuestionIndex);
        }
    }

    // Геттеры для доступа из других скриптов
    public string[] GetQuestions() => questions;
    public string[] GetAnswers() => answers;
    public string[] GetPositions() => positions;
    public string[] GetIsCorrect() => isCorrect;
    public int GetCurrentQuestionIndex() => currentQuestionIndex;
    public int GetTotalQuestions() => questions?.Length ?? 0;
    public string GetCurrentLevelPath() => currentLevelPath;
}