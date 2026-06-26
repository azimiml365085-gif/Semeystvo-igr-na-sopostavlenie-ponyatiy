using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class AnswerButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectToDelete;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Text legacyText;
    [SerializeField] private Toggle isCorrectToggle; // Toggle для выбора правильности

    [Header("File Settings")]
    [SerializeField] private string answersFileName = "Answers.txt";
    [SerializeField] private string positionsFileName = "AnswerPositions.txt";
    [SerializeField] private string correctFileName = "AnswerIsCorrect.txt";

    private string LevelFolderPath => LevelManager.currentLevelPath;

    private string AnswersFilePath => string.IsNullOrEmpty(LevelFolderPath)
        ? Path.Combine(Application.persistentDataPath, answersFileName)
        : Path.Combine(LevelFolderPath, answersFileName);

    private string PositionsFilePath => string.IsNullOrEmpty(LevelFolderPath)
        ? Path.Combine(Application.persistentDataPath, positionsFileName)
        : Path.Combine(LevelFolderPath, positionsFileName);

    private string CorrectFilePath => string.IsNullOrEmpty(LevelFolderPath)
        ? Path.Combine(Application.persistentDataPath, correctFileName)
        : Path.Combine(LevelFolderPath, correctFileName);

    public void OnButtonClick()
    {
        // Создаем папку если нужно
        if (!string.IsNullOrEmpty(LevelFolderPath) && !Directory.Exists(LevelFolderPath))
        {
            Directory.CreateDirectory(LevelFolderPath);
        }

        // Получаем данные ДО удаления
        string answerText = inputField != null ? inputField.text : "";
        Vector3 position = objectToDelete != null ? objectToDelete.transform.position : Vector3.zero;
        bool isCorrect = isCorrectToggle != null ? isCorrectToggle.isOn : false;

        // Сохраняем ответ (одна строка)
        SaveAnswer(answerText);

        // Сохраняем позицию (одна строка)
        SavePosition(position);

        // Сохраняем правильность (1 или 0)
        SaveIsCorrect(isCorrect);

        // Выводим в Legacy Text
        if (legacyText != null)
        {
            legacyText.text = answerText;
        }

        // Удаляем объект
        if (objectToDelete != null)
        {
            Destroy(objectToDelete);
        }

        Debug.Log($"✅ Saved: {answerText} | Position: {position} | IsCorrect: {isCorrect}");
    }

    private void SaveAnswer(string answer)
    {
        try
        {
            File.AppendAllText(AnswersFilePath, answer + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save answer: {e.Message}");
        }
    }

    private void SavePosition(Vector3 position)
    {
        try
        {
            string positionString = $"{position.x:F2},{position.y:F2},{position.z:F2}";
            File.AppendAllText(PositionsFilePath, positionString + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save position: {e.Message}");
        }
    }

    private void SaveIsCorrect(bool isCorrect)
    {
        try
        {
            string value = isCorrect ? "1" : "0";
            File.AppendAllText(CorrectFilePath, value + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save isCorrect: {e.Message}");
        }
    }
}