using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    [Header("Cursor")]
    [SerializeField] private Texture2D cursor;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 tmp = new Vector2(10f, 10f);
        Cursor.SetCursor(cursor, tmp, CursorMode.ForceSoftware);
    }

}
