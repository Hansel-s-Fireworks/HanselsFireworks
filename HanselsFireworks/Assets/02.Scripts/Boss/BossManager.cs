using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    private bool playerCanAttack;
    public UnityEvent PhaseStart;
    public UnityEvent<bool> PhaseEndEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
                Destroy(this.gameObject);
        }

        playerCanAttack = false;
    }

    private void Start()
    {
        Phase1Start();
    }

    private void PhaseOneEnd()
    {
        if (PumkinManager.Instance.GetPumkinList().Count == 0)
        {
            Debug.Log("Clear");
            playerCanAttack = true;
        }
        else
        {
            Debug.Log("fail");
            Phase1Start();
            playerCanAttack = false;
        }
        PhaseEndEvent.Invoke(playerCanAttack);
    }

    private void Phase1Start()
    {
        PhaseStart.Invoke();
        //PumkinManager.Instance.DeleteAllPumkin();
        Invoke("PhaseOneEnd", 10f);
    }

}
