using UnityEngine;

public class mouseCursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextures;
    public int currentCursorIndex = -1;
    public int stampIndex = 0;


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
        2: stamp press
        3: stamp forward
        4: stamp supervisor
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
                stampIndex = 0;
                index = 2;
                break;
            case 3:
            {
                Cursor.SetCursor(cursorTextures[2], new Vector2(32, 64), CursorMode.Auto);
                stampIndex = 1;
                index = 2;
                break;
            }
            case 4:
            {
                Cursor.SetCursor(cursorTextures[2], new Vector2(32, 64), CursorMode.Auto);
                stampIndex = 2;
                index = 2;
                break;
            }
            default:
                Debug.LogWarning("Invalid cursor index");
                break;
        }
        currentCursorIndex = index;
    }
}
