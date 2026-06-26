using UnityEngine;

public class CameraScrollController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 10f;
    public float smoothTime = 0.1f;

    [Header("Границы уровня")]
    public float minX = -20f;
    public float maxX = 20f;

    [Header("Управление мышью (у краёв)")]
    public float edgeThreshold = 30f;
    public bool enableMouseEdgeScroll = true;

    [Header("Управление клавишами")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private bool isScrolling = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        float moveDirection = 0f;

        // ---- 1. Управление клавишами ----
        if (Input.GetKey(moveLeftKey) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1f;
        }
        else if (Input.GetKey(moveRightKey) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1f;
        }

        // ---- 2. Управление мышью у краёв ----
        if (enableMouseEdgeScroll && moveDirection == 0f)
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.x < edgeThreshold)
            {
                moveDirection = -1f;
            }
            else if (mousePos.x > Screen.width - edgeThreshold)
            {
                moveDirection = 1f;
            }
        }

        // ---- 3. Применяем движение ----
        if (moveDirection != 0f)
        {
            targetPosition.x += moveDirection * moveSpeed * Time.deltaTime;
            isScrolling = true;
        }
        else
        {
            isScrolling = false;
        }

        // ---- 4. Ограничиваем границы ----
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = transform.position.y;
        targetPosition.z = transform.position.z;

        // ---- 5. Плавное движение ----
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }

    // ---- Визуализация границ в редакторе ----
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minX, -100, 0), new Vector3(minX, 100, 0));
        Gizmos.DrawLine(new Vector3(maxX, -100, 0), new Vector3(maxX, 100, 0));
    }

    public float GetCurrentX()
    {
        return transform.position.x;
    }
}