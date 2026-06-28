using UnityEngine;
using UnityEngine.UI;

public class MeteorUser : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isCorrect = false;
    [SerializeField] private int answerIndex = -1;

    [Header("Components")]
    [SerializeField] private Text textComponent;

    private bool isDestroyed = false;

    void Update()
    {
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        if (transform.position.x < leftEdge - 2f && !isDestroyed)
        {
            isDestroyed = true;
            UserLevelLoader.Instance?.OnMeteorMissed();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser" && !isDestroyed)
        {
            isDestroyed = true;

            if (isCorrect)
            {
                GameManagerCSUser.score += 1;
                Debug.Log($"✅ +1 очко! Счет: {GameManagerCSUser.score}");
            }
            else
            {
                GameManagerCSUser.score -= 1;
                Debug.Log($"❌ -1 очко! Счет: {GameManagerCSUser.score}");
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);

            if (isCorrect)
            {
                UserLevelLoader.Instance?.OnCorrectAnswer();
            }
        }
    }

    public void SetCorrect(bool correct) => isCorrect = correct;
    public void SetAnswerIndex(int index) => answerIndex = index;
    public void SetText(string text)
    {
        if (textComponent == null)
            textComponent = GetComponentInChildren<Text>();
        if (textComponent != null)
            textComponent.text = text;
    }
}