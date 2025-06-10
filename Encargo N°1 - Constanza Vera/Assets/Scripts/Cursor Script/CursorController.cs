using UnityEngine;


public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    private Vector2 cursorHotspot;
    void Start()
    {
        cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }

}
