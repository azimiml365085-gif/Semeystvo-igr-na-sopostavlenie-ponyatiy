using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TextFileReader : MonoBehaviour
{
    public Text legacyText;
    public string filePath = "BaseLevels/CosmicStranger/Questions.txt"; // Путь относительно папки с игрой

    // Объекты для сравнения координат X
    public GameObject objectA;
    public GameObject[] objectsB; // Массив объектов B

    private string[] lines;
    private int index = 0;
    private bool allQuestionsAnswered = false;
    private bool hasTriggered = false; // Флаг для предотвращения повторных срабатываний

    // Статическая ссылка на себя
    private static TextFileReader instance;

    void Start()
    {
        // Сохраняем ссылку на себя
        instance = this;

        // Полный путь к файлу
        string fullPath = Path.Combine(Application.dataPath, filePath);

        try
        {
            if (File.Exists(fullPath))
            {
                // Читаем файл
                string fileContent = File.ReadAllText(fullPath);
                lines = fileContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                UpdateText();
            }
            else
            {
                legacyText.text = $"Файл не найден по пути: {fullPath}";
                Debug.LogError($"Файл не найден: {fullPath}");
            }
        }
        catch (System.Exception e)
        {
            legacyText.text = "Ошибка чтения файла!";
            Debug.LogError($"Ошибка чтения файла: {e.Message}");
        }
    }

    void Update()
    {
        // Проверяем, не стал ли X объекта A больше X любого из объектов B
        if (objectA != null && objectsB != null && objectsB.Length > 0 && !hasTriggered)
        {
            foreach (GameObject objB in objectsB)
            {
                if (objB != null && objectA.transform.position.x > objB.transform.position.x)
                {
                    NextLine();
                    hasTriggered = true; // Блокируем повторные срабатывания
                    break;
                }
            }
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
            // Проверяем, не закончились ли вопросы
            if (index < lines.Length - 1)
            {
                index++;
                UpdateText();
                // Сбрасываем флаг для следующего вопроса
                hasTriggered = false;
            }
            else if (!allQuestionsAnswered)
            {
                // Если дошли до последнего вопроса, показываем финальное сообщение
                allQuestionsAnswered = true;
                legacyText.text = "Вы ответили на все вопросы!";
                // Отключаем проверку, так как все вопросы завершены
                enabled = false;
            }
            // Если уже показано финальное сообщение, ничего не делаем
        }
    }

    void UpdateText()
    {
        legacyText.text = lines[index];
    }

    // Публичный метод для ручного сброса проверки
    public void ResetTrigger()
    {
        hasTriggered = false;
        enabled = true;
    }

    // Публичный метод для принудительного переключения на следующий вопрос через позицию
    public void CheckPosition()
    {
        if (objectA != null && objectsB != null && objectsB.Length > 0)
        {
            foreach (GameObject objB in objectsB)
            {
                if (objB != null && objectA.transform.position.x > objB.transform.position.x)
                {
                    NextLine();
                    break;
                }
            }
        }
    }
}