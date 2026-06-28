using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private Font font;

    public static string SelectedLevelPath;
    private string fullPath;
    private List<GameObject> buttons = new List<GameObject>();

    void Start()
    {
        fullPath = Path.Combine(Application.dataPath, "UserLevels/CosmicStranger");
        LoadLevels();
    }

    public void LoadLevels()
    {
        ClearButtons();

        if (!Directory.Exists(fullPath))
        {
            Debug.LogWarning($"Папка не найдена: {fullPath}");
            return;
        }

        string[] folders = Directory.GetDirectories(fullPath);

        if (folders.Length == 0)
        {
            Debug.Log("Нет папок в указанной директории");
            return;
        }

        System.Array.Sort(folders);

        RectTransform containerRect = buttonsContainer.GetComponent<RectTransform>();
        if (containerRect != null)
        {
            containerRect.anchoredPosition = Vector2.zero;
        }

        for (int i = 0; i < folders.Length; i++)
        {
            string folderName = Path.GetFileName(folders[i]);
            CreateButtonFromCode(folderName, folders[i], i);
        }

        UpdateContainerSize();
        Debug.Log($"Загружено {folders.Length} папок");
    }

    private void CreateButtonFromCode(string folderName, string fullPath, int index)
    {
        GameObject buttonObj = new GameObject($"Button_{folderName}");
        buttonObj.transform.SetParent(buttonsContainer, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);

        float buttonHeight = 30f;
        float spacing = 5f;
        float yPos = -index * (buttonHeight + spacing);

        rect.anchoredPosition = new Vector2(0, yPos);
        rect.sizeDelta = new Vector2(0, buttonHeight);

        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        colors.pressedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        button.colors = colors;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 0);
        textRect.offsetMax = new Vector2(-10, 0);

        Text text = textObj.AddComponent<Text>();
        text.text = folderName;
        text.font = font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 14;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = Color.white;

        string path = fullPath;
        button.onClick.AddListener(() => OnButtonClick(path));

        buttons.Add(buttonObj);
    }

    private void OnButtonClick(string levelPath)
    {
        Debug.Log($"Выбрана папка: {levelPath}");
        SelectedLevelPath = levelPath;
        SceneManager.LoadScene("CosmicStrangerUser");
    }

    private void UpdateContainerSize()
    {
        if (buttonsContainer == null) return;

        RectTransform containerRect = buttonsContainer.GetComponent<RectTransform>();
        if (containerRect != null)
        {
            float buttonHeight = 30f;
            float spacing = 5f;
            float totalHeight = buttons.Count * (buttonHeight + spacing) + spacing;
            containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, totalHeight);
        }
    }

    private void ClearButtons()
    {
        foreach (GameObject btn in buttons)
        {
            if (btn != null)
                DestroyImmediate(btn);
        }
        buttons.Clear();
    }
}