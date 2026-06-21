using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed = 14f;
    void Update()
    {
        Vector2 rightBound = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));

        if (transform.position.x > rightBound.x)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Meteor")
        {
            Destroy(gameObject);
        }
    }
}
