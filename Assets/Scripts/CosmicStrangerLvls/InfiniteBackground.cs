using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;        // Скорость движения
    public float spriteWidth = 10f;        // Ширина одного тайла

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Автоматически берём ширину спрайта
        spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // Растягиваем фон на 3 экрана, чтобы не было разрывов
        spriteRenderer.size = new Vector2(spriteWidth * 3, spriteRenderer.size.y);
    }

    void Update()
    {
        // Двигаем фон влево
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // Если фон ушёл влево больше, чем на ширину одного тайла,
        // перебрасываем его вправо на один тайл (создаём бесконечность)
        if (transform.position.x < -spriteWidth)
        {
            transform.position += Vector3.right * spriteWidth;
        }
    }
}