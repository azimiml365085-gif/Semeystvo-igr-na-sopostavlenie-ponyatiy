using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private float delay = 3f; // Время до отключения

    private void OnEnable()
    {
        // Запускаем корутину при активации объекта
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        // Ждём указанное количество секунд
        yield return new WaitForSeconds(delay);

        // Отключаем объект
        gameObject.SetActive(false);
    }

    // Опционально: если нужно сбросить корутину при отключении
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}