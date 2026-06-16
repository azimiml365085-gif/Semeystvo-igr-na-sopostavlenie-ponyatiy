using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    private float timer = 0f;
    public Vector2 rightBound;

    void Update()
    {
        rightBound = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));

        // Уничтожаем, если улетел за левый край

        timer += Time.deltaTime;
        if (timer >= Random.Range(1f, 2f))
        {
            timer = 0f;
            SpawnMeteorite();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            Destroy(gameObject);
        }
    }

    void SpawnMeteorite()
    {

        // Создаём метеорит в случайной позиции справа за экраном
        Vector2 spawnPos = new Vector2(rightBound.x, Random.Range(-1f, 4f));
        GameObject newMeteorite = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}
