using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private BoxCollider2D boxCollider;
    private Camera mainCamera;
    private float minY, maxY;
    private ContactFilter2D contactFilter;
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject gameOverText;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;
        gameOverText.SetActive(false);

        // Находим границы с помощью коллайдера камеры
        CreateCameraBounds();
    }

    void CreateCameraBounds()
    {
        // Получаем размеры камеры
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Учитываем размер корабля
        float shipHeight = boxCollider.bounds.size.y;

        // Центр камеры в мировых координатах
        Vector2 cameraCenter = mainCamera.transform.position;

        minY = cameraCenter.y - cameraHeight / 2f + shipHeight / 2f + 3f;
        maxY = cameraCenter.y + cameraHeight / 2f - shipHeight / 2f;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(0, vertical * speed * Time.deltaTime);

        // Проверяем, не выйдет ли корабль за границы после движения
        Vector2 potentialPosition = (Vector2)transform.position + movement;
        potentialPosition.y = Mathf.Clamp(potentialPosition.y, minY, maxY);

        transform.position = potentialPosition;

        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(laser, shootPos.position, transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Meteor")
        {
            Destroy(gameObject);
            Destroy(spawner);
            gameOverText.SetActive(true);
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Meteor");
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
            Time.timeScale = 0f;
        }
    }
}