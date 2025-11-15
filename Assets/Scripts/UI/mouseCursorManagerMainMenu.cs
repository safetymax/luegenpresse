using UnityEngine;

public class mouseCursorManagerMainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

    }
}
