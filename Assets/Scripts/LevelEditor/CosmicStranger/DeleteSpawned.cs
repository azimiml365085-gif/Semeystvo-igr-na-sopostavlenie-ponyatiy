using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnableObject : MonoBehaviour
{
    [Header("Настройки")]
    public bool canBeDeleted = true;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && canBeDeleted)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Destroy(gameObject);
                Debug.Log("Объект удалён: " + gameObject.name);
            }
        }
    }
}