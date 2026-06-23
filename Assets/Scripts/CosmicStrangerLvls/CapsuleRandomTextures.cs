using UnityEngine;

public class CapsuleRandomTexture : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    // Эта опция позволит тебе выбрать, менять ли текстуру при старте
    [SerializeField] private bool randomizeOnStart = true;

    void Awake()
    {
        if (randomizeOnStart)
            SetRandomSprite();
    }

    // Этот метод можно вызывать вручную, если нужно
    public void SetRandomSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Нет спрайтов для выбора!", this);
            return;
        }

        int randomIndex = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
    }
}