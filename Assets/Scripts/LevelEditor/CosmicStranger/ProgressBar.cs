using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    [Header("Ссылки")]
    public Slider progressSlider;               // Ссылка на слайдер
    public CameraScrollController cameraController; // Ссылка на скрипт камеры
    public Transform rightEdgeObject;           // Объект-маркер правого края

    [Header("Настройки")]
    public float startX = 0f;                   // Стартовая позиция камеры (X)
    public bool smoothUpdate = true;            // Плавное обновление
    public float smoothSpeed = 5f;              // Скорость плавности

    private float currentProgress = 0f;         // Текущее значение (0-1)
    private float totalDistance;                // Расстояние от старта до края

    void Start()
    {
        // Если слайдер не назначен — ищем на сцене
        if (progressSlider == null)
            progressSlider = FindObjectOfType<Slider>();

        // Если камера не назначена — ищем на Main Camera
        if (cameraController == null)
            cameraController = Camera.main.GetComponent<CameraScrollController>();

        // Настраиваем слайдер (от 0 до 1)
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
        progressSlider.wholeNumbers = false;

        // Начальное значение
        currentProgress = 0f;
        progressSlider.value = 0f;

        // Рассчитываем общее расстояние от старта до правого края
        if (rightEdgeObject != null)
        {
            totalDistance = Mathf.Abs(rightEdgeObject.position.x - startX);
        }
        else
        {
            Debug.LogWarning("RightEdgeObject не назначен!");
            totalDistance = 1f; // Чтобы избежать деления на 0
        }
    }

    void Update()
    {
        if (cameraController == null || progressSlider == null || rightEdgeObject == null) return;

        // ---- 1. Рассчитываем прогресс ----
        float rawProgress = CalculateProgress();

        // ---- 2. Применяем к слайдеру ----
        if (smoothUpdate)
        {
            currentProgress = Mathf.Lerp(currentProgress, rawProgress, smoothSpeed * Time.deltaTime);
            progressSlider.value = currentProgress;
        }
        else
        {
            currentProgress = rawProgress;
            progressSlider.value = currentProgress;
        }
    }

    // ---- Рассчитать прогресс (0 = старт, 1 = правый край) ----
    float CalculateProgress()
    {
        float cameraX = cameraController.GetCurrentX();
        float rightX = rightEdgeObject.position.x;

        // Текущее расстояние от камеры до правого края
        float distanceToRight = Mathf.Abs(rightX - cameraX);

        // Прогресс: чем ближе к правому краю, тем больше (1 = достигнут правый край)
        float progress = 1f - (distanceToRight / totalDistance);

        return Mathf.Clamp01(progress);
    }

    // ---- Публичные методы ----
    public void SetProgress(float value) // 0..1
    {
        currentProgress = Mathf.Clamp01(value);
        progressSlider.value = currentProgress;
    }

    public float GetProgress()
    {
        return currentProgress;
    }
}