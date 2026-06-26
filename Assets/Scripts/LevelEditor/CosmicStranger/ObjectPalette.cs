using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPalette : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки")]
    public GameObject prefabToSpawn;
    public bool isSingleInstance = false;

    private GameObject spawnedObject;
    private Camera mainCamera;
    private GameObject existingInstance;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefabToSpawn == null) return;

        if (isSingleInstance)
        {
            existingInstance = GameObject.Find(prefabToSpawn.name + "(Clone)");
            if (existingInstance != null)
            {
                Destroy(existingInstance);
                existingInstance = null;
            }
        }

        Vector3 spawnPos = GetMouseWorldPos();
        spawnPos.z = 0;
        spawnedObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedObject == null) return;

        Vector3 newPos = GetMouseWorldPos();
        newPos.z = 0;
        spawnedObject.transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawnedObject = null;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}