using UnityEngine;
using UnityEngine.UI;

public class MeteorTextController : MonoBehaviour
{
    [Header("Настройки текста")]
    public string displayText = "Текст на объекте";
    public Font font;
    public int fontSize = 20;
    public Color textColor = Color.yellow;
    public Vector3 textOffset = new Vector3(0, 1, 0);
    public bool followObject = true; // Следить за объектом

    private Text textComponent;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform targetObject;

    void Start()
    {
        // Находим или создаём Canvas
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Создаём текст
        CreateText();

        // Сохраняем ссылку на объект, к которому привязаны
        targetObject = transform;
    }

    void CreateText()
    {
        GameObject textGO = new GameObject("TextOn_" + gameObject.name);
        textGO.transform.SetParent(canvas.transform, false);

        textComponent = textGO.AddComponent<Text>();
        textComponent.text = displayText;
        textComponent.font = font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = fontSize;
        textComponent.color = textColor;
        textComponent.alignment = TextAnchor.MiddleCenter;

        rectTransform = textGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 100);

        UpdateTextPosition();
    }

    void Update()
    {
        if (followObject && targetObject != null && textComponent != null)
        {
            UpdateTextPosition();
        }
    }

    void UpdateTextPosition()
    {
        if (targetObject == null || textComponent == null) return;

        Vector3 worldPos = targetObject.position + textOffset;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rectTransform.position = screenPos;
    }

    public void SetText(string newText)
    {
        displayText = newText;
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
    }
}