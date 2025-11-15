using UnityEngine;
using UnityEngine.InputSystem;

public class worldCursor : MonoBehaviour
{
    public Camera cam;
    public LayerMask hitMask;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hitMask))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y, -10);
        }
    }
}
