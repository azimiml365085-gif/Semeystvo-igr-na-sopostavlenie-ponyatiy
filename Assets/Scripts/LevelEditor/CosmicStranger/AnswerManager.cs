using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using System.Globalization;

public class AnswerButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectToDelete;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Text legacyText;
    [SerializeField] private Toggle isCorrectToggle;

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
        if (!string.IsNullOrEmpty(LevelFolderPath) && !Directory.Exists(LevelFolderPath))
        {
            Directory.CreateDirectory(LevelFolderPath);
        }

        string answerText = inputField != null ? inputField.text : "";
        Vector3 position = objectToDelete != null ? objectToDelete.transform.position : Vector3.zero;
        bool isCorrect = isCorrectToggle != null ? isCorrectToggle.isOn : false;

        SaveAnswer(answerText);
        SavePosition(position);
        SaveIsCorrect(isCorrect);

        if (legacyText != null)
        {
            legacyText.text = answerText;
        }

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
            // Используем InvariantCulture для сохранения с ТОЧКОЙ (а не запятой)
            string positionString = $"{position.x.ToString("F2", CultureInfo.InvariantCulture)}," +
                                    $"{position.y.ToString("F2", CultureInfo.InvariantCulture)}," +
                                    $"{position.z.ToString("F2", CultureInfo.InvariantCulture)}";
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