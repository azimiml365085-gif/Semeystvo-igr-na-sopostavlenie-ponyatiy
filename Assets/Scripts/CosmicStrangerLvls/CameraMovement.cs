using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.right * 2f * Time.deltaTime);
    }
}
