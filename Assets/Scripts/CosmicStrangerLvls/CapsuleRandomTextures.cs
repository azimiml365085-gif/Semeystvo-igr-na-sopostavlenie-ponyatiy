using UnityEngine;

public class CapsuleRandomTextures : MonoBehaviour
{
    [SerializeField] private Texture2D[] textures; // Сюда перетащи 3 текстуры

    void Start()
    {
        // Выбираем случайный индекс (0, 1 или 2)
        int randomIndex = Random.Range(0, textures.Length);

        // Применяем текстуру к компоненту Renderer
        GetComponent<Renderer>().material.mainTexture = textures[randomIndex];
    }
}
