using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Phase2Manager : MonoBehaviour
{
    public static Phase2Manager Instance;

    public int snackCnt = 8;

    [SerializeField]
    private List<GameObject> breakableWalls;

    [SerializeField]
    private AudioSource wallBreak;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (snackCnt == 0)
        {
            snackCnt--;
            addScriptToBreakableWalls();
        }
    }


    public void addScriptToBreakableWalls()
    {
        foreach (GameObject walls in breakableWalls)
        {
            walls.AddComponent<BreakableWall>();
            walls.GetComponent<BreakableWall>().damageSound = wallBreak;
        }
    }
}
