using UnityEngine;
using UnityEngine.InputSystem;

public class worldCursor : MonoBehaviour
{
    public Camera cam;
    public LayerMask hitMask;

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }
}
