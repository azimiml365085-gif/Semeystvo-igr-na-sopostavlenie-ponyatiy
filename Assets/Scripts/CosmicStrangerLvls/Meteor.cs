using UnityEngine;

public class Meteor : MonoBehaviour
{

    void Update()
    {
       // Уничтожаем, если улетел за левый край
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        if (transform.position.x < leftEdge - 2f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            Destroy(gameObject);

        }
    }
}