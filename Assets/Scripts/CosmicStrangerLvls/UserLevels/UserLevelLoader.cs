using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

public class UserLevelLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text questionText;
    [SerializeField] private Text levelNameText;

    [Header("Objects")]
    [SerializeField] private GameObject meteorPrefab;

    [Header("Settings")]
    [SerializeField] private int maxAnswersPerQuestion = 4;
    [SerializeField] private float yOffset = 1f; // Поднимает все метеоры на 1 вверх
    [SerializeField] private float panelOffset = 3f; // Отступ от нижней панели

    private string[] questions;
    private string[] answers;
    private string[] positions;
    private string[] isCorrect;

    private int currentQuestionIndex = 0;
    private List<GameObject> spawnedMeteors = new List<GameObject>();
    private bool isLevelComplete = false;
    private bool isWaiting = false;

    private Camera mainCamera;
    private float minY;
    private float maxY;

    public static UserLevelLoader Instance { get; private set; }

    void Start()
    {
        Instance = this;

        mainCamera = Camera.main;
        CalculateCameraBounds();

        string levelPath = LevelSelector.SelectedLevelPath;

        if (string.IsNullOrEmpty(levelPath))
        {
            Debug.LogError("Путь к уровню не передан!");
            return;
        }

        LoadLevel(levelPath);
    }

    private void CalculateCameraBounds()
    {
        if (mainCamera == null) return;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        Vector2 cameraCenter = mainCamera.transform.position;

        // Точные границы с учетом нижней панели (как в PlayerController)
        minY = cameraCenter.y - cameraHeight / 2f + panelOffset;
        maxY = cameraCenter.y + cameraHeight / 2f;

        Debug.Log($"Границы камеры: minY={minY}, maxY={maxY}");
    }

    public void LoadLevel(string path)
    {
        ClearMeteors();

        isLevelComplete = false;
        isWaiting = false;
        currentQuestionIndex = 0;
        GameManagerCSUser.score = 0;

        LoadFiles(path);

        string levelName = Path.GetFileName(path);
        if (levelNameText != null)
            levelNameText.text = levelName;

        ShowQuestion(0);

        Debug.Log($"=== Загружен уровень: {levelName} ===");
    }

    private void LoadFiles(string path)
    {
        string questionsPath = Path.Combine(path, "Questions.txt");
        string answersPath = Path.Combine(path, "Answers.txt");
        string positionsPath = Path.Combine(path, "AnswerPositions.txt");
        string isCorrectPath = Path.Combine(path, "AnswerIsCorrect.txt");

        questions = File.ReadAllLines(questionsPath);
        answers = File.ReadAllLines(answersPath);
        positions = File.ReadAllLines(positionsPath);
        isCorrect = File.ReadAllLines(isCorrectPath);
    }

    private void ShowQuestion(int index)
    {
        if (questions == null || index >= questions.Length)
        {
            isLevelComplete = true;
            if (questionText != null)
                questionText.text = "🎉 Все вопросы пройдены!";
            return;
        }

        if (questionText != null)
            questionText.text = questions[index];

        ClearMeteors();
        isWaiting = false;

        int startIndex = index * maxAnswersPerQuestion;
        int endIndex = Mathf.Min(startIndex + maxAnswersPerQuestion, answers.Length);

        for (int i = startIndex; i < endIndex; i++)
        {
            CreateMeteor(i);
        }

        Debug.Log($"Вопрос {index}: создано {endIndex - startIndex} метеоров");
    }

    private void CreateMeteor(int index)
    {
        if (meteorPrefab == null)
        {
            Debug.LogError("MeteorPrefab не назначен!");
            return;
        }

        GameObject meteor = Instantiate(meteorPrefab);

        Vector3 worldPos = ParsePosition(positions[index]);
        worldPos.z = 0;

        // Поднимаем на 1 вверх
        worldPos.y += yOffset;

        // Ограничиваем по Y (чтобы не выходил за границы)
        worldPos.y = Mathf.Clamp(worldPos.y, minY, maxY);

        meteor.transform.position = worldPos;

        Debug.Log($"Метеор {index}: позиция {worldPos} (ограничено: {minY} - {maxY})");

        Text meteorText = meteor.GetComponentInChildren<Text>();
        if (meteorText != null)
            meteorText.text = answers[index];

        MeteorUser meteorScript = meteor.GetComponent<MeteorUser>();
        if (meteorScript != null)
        {
            bool correct = isCorrect[index].Trim() == "1";
            meteorScript.SetCorrect(correct);
            meteorScript.SetAnswerIndex(index);
        }

        spawnedMeteors.Add(meteor);
    }

    private Vector3 ParsePosition(string positionStr)
    {
        string[] parts = positionStr.Replace(" ", "").Split(',');

        if (parts.Length >= 3)
        {
            float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[2], CultureInfo.InvariantCulture);
            return new Vector3(x, y, 0);
        }

        return Vector3.zero;
    }

    public void OnCorrectAnswer()
    {
        if (isLevelComplete || isWaiting) return;

        Debug.Log("✅ Правильный ответ! Переход к следующему вопросу");
        isWaiting = true;
        Invoke("NextQuestion", 0.3f);
    }

    public void OnMeteorMissed()
    {
        if (isLevelComplete || isWaiting) return;

        Debug.Log("⏰ Метеор улетел! Переход к следующему вопросу");
        isWaiting = true;
        Invoke("NextQuestion", 0.3f);
    }

    private void NextQuestion()
    {
        ClearMeteors();
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            isLevelComplete = true;
            if (questionText != null)
                questionText.text = "🎉 Все вопросы пройдены!";
            Debug.Log("Все вопросы завершены!");
        }
    }

    private void ClearMeteors()
    {
        foreach (GameObject meteor in spawnedMeteors)
        {
            if (meteor != null)
                Destroy(meteor);
        }
        spawnedMeteors.Clear();
    }
}