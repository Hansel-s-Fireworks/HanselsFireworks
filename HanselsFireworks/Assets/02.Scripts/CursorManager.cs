using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CursorManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    private bool cursorHidden = false;

    void Start()
    {
        HideCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // 예를 들면 ESC 키를 눌렀을 때
        {
            ToggleCursor();
        }
    }

    private void HideCursor()
    {
        ShowCursor(false);
        cursorHidden = true;
    }

    private void ShowCursor()
    {
        ShowCursor(true);
        cursorHidden = false;
    }

    private void ToggleCursor()
    {
        if (cursorHidden)
        {
            ShowCursor();
        }
        else
        {
            HideCursor();
        }
    }
}
