using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Transform player;        // Трансформ игрока
    [SerializeField] private Transform endPoint;      // Трансформ конечной точки
    [SerializeField] private Slider progressSlider;   // Слайдер для прогресса
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject menuPanel;


    private float startX;    // Стартовая позиция игрока по X
    private float endX;      // Позиция конечной точки по X
    private float totalDistance; // Общее расстояние от старта до финиша

    void Start()
    {

        victoryText.SetActive(false);
        // Запоминаем стартовую позицию игрока по X
        startX = player.position.x;

        // Запоминаем позицию конечной точки по X
        endX = endPoint.position.x;

        // Вычисляем общее расстояние
        totalDistance = endX - startX;

        // Убеждаемся, что прогресс не отрицательный
        if (totalDistance <= 0)
        {
            totalDistance = 1f; // Чтобы избежать деления на ноль
        }

        // Сбрасываем слайдер на 0
        progressSlider.value = 0f;
        
        // Подписываемся на событие изменения значения
        progressSlider.onValueChanged.AddListener(OnProgressChanged);
    }

    void OnProgressChanged(float value)
    {
        if (value >= 1f)
        {
            if (GameManagerCS.score >= 4)
            {
                menuPanel.SetActive(true);
                victoryText.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                menuPanel.SetActive(true);
                gameOverText.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    void OnDestroy()
    {
        // Отписываемся, чтобы избежать ошибок
        progressSlider.onValueChanged.RemoveListener(OnProgressChanged);
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