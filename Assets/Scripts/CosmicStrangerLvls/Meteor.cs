using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    public bool isCorrect = false;

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
            if (isCorrect)
            {
                GameManagerCS.score += 1;
                TextFileReader.ChangeText();

            }
            else
            {
                GameManagerCS.score -= 1;
            }

        }
    }
}