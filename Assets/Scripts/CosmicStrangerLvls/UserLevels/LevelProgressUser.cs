using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUser : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject menuPanel;

    private float startX;
    private float endX;
    private float totalDistance;

    void Start()
    {
        victoryText.SetActive(false);
        startX = player.position.x;
        endX = endPoint.position.x;
        totalDistance = endX - startX;

        if (totalDistance <= 0)
        {
            totalDistance = 1f;
        }

        progressSlider.value = 0f;
        progressSlider.onValueChanged.AddListener(OnProgressChanged);
    }

    void OnProgressChanged(float value)
    {
        if (value >= 1f)
        {
            if (GameManagerCSUser.score >= 4)
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
        progressSlider.onValueChanged.RemoveListener(OnProgressChanged);
    }

    void Update()
    {
        float currentX = player.position.x;
        float progress = (currentX - startX) / totalDistance;
        progress = Mathf.Clamp01(progress);
        progressSlider.value = progress;
    }
}