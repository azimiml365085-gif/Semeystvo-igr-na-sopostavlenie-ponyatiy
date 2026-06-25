using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public Text legacyText;
    public string resourcePath = "CosmicStrangerQuestions/Questions";

    private string[] lines;
    private int index = 0;

    // Статическая ссылка на себя
    private static QuestionManager instance;

    void Start()
    {
        // Сохраняем ссылку на себя
        instance = this;

        TextAsset textFile = Resources.Load<TextAsset>(resourcePath);
        if (textFile != null)
        {
            lines = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            UpdateText();
        }
        else
        {
            legacyText.text = "Файл не найден!";
        }
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
            Debug.LogWarning("TextFileReader не существует на сцене!");
        }
    }

    public void NextLine()
    {
        if (lines != null && lines.Length > 0)
        {
            index = (index + 1) % lines.Length;
            UpdateText();
        }
    }

    void UpdateText()
    {
        legacyText.text = lines[index];
    }
}