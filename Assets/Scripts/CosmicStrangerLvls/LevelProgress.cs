using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Transform player;        // Трансформ игрока
    [SerializeField] private Transform endPoint;      // Трансформ конечной точки
    [SerializeField] private Slider progressSlider;   // Слайдер для прогресса

    private float startX;    // Стартовая позиция игрока по X
    private float endX;      // Позиция конечной точки по X
    private float totalDistance; // Общее расстояние от старта до финиша

    void Start()
    {
        // Запоминаем стартовую позицию игрока по X
        startX = player.position.x;

        // Запоминаем позицию конечной точки по X
        endX = endPoint.position.x;

        // Вычисляем общее расстояние
        totalDistance = endX - startX;

        // Убеждаемся, что прогресс не отрицательный
        if (totalDistance <= 0)
        {
            Debug.LogWarning("EndPoint находится левее или на той же позиции, что и игрок! Проверь сцену.");
            totalDistance = 1f; // Чтобы избежать деления на ноль
        }

        // Сбрасываем слайдер на 0
        progressSlider.value = 0f;
    }

    void Update()
    {
        // Текущая позиция игрока по X
        float currentX = player.position.x;

        // Сколько процентов пути пройдено
        float progress = (currentX - startX) / totalDistance;

        // Ограничиваем значение от 0 до 1 (чтобы не выходило за пределы)
        progress = Mathf.Clamp01(progress);

        // Обновляем слайдер
        progressSlider.value = progress;
    }
}