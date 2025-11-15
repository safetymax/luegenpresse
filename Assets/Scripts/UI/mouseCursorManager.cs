using UnityEngine;

public class mouseCursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextures;
    public int currentCursorIndex = -1;


    private void Start()
    {
        // Set default cursor
        //setCursor(2);
    }

    public void setCursor(int index)
    {
        /*
        -1: default
        0: letter opener
        1: marker
        2: stamp
        */
       switch (index)
        {
            case -1:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case 0:
                Cursor.SetCursor(cursorTextures[0], new Vector2(0,64), CursorMode.Auto);
                break;
            case 1:
                Cursor.SetCursor(cursorTextures[1], new Vector2(0, 64), CursorMode.Auto);
                break;
            case 2:
                Cursor.SetCursor(cursorTextures[2], new Vector2(32, 64), CursorMode.Auto);
                break;
            default:
                Debug.LogWarning("Invalid cursor index");
                break;
        }
        currentCursorIndex = index;
    }
}
