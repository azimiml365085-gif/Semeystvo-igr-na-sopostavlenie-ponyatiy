using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.right * 5f * Time.deltaTime);
    }
}
