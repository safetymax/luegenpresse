using UnityEngine;
using UnityEngine.InputSystem;

public class HoverRaycast : MonoBehaviour
{
    public float liftAmount = 0.2f;
    public float speed = 10f;

    private Vector3 originalPos;
    private Vector3 targetPos;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("HoverRaycast: No MainCamera found! Add the MainCamera tag.");

        originalPos = transform.position;
        targetPos = originalPos;
    }

    void Update()
    {
        if (Mouse.current == null || cam == null)
            return;

        Vector2 mouse = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouse);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
                targetPos = originalPos + Vector3.up * liftAmount;
            else
                targetPos = originalPos;
        }
        else
        {
            targetPos = originalPos;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }
}
