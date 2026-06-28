using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isCorrect;
    [SerializeField] private int answerIndex;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    public System.Action<bool, int> OnAnswerSelected;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCorrect(bool correct)
    {
        isCorrect = correct;
    }

    public void SetIndex(int index)
    {
        answerIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCorrect)
        {
            Debug.Log($"✅ Правильный ответ! Индекс: {answerIndex}");
            if (spriteRenderer != null)
                spriteRenderer.color = correctColor;
            OnAnswerSelected?.Invoke(true, answerIndex);
        }
        else
        {
            Debug.Log($"❌ Неправильный ответ! Индекс: {answerIndex}");
            if (spriteRenderer != null)
                spriteRenderer.color = wrongColor;
            OnAnswerSelected?.Invoke(false, answerIndex);
        }
    }

    public void ResetColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;
    }
}