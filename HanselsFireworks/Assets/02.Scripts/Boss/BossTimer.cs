using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTimer : MonoBehaviour
{
    public void Start()
    {
        BossManager.instance.PhaseStartEvent.AddListener(PhaseStart);
        if (BossManager.instance.currentPhase == 3)
        {
            PhaseStart(3);
        }
    }

    public void PhaseStart(int phase)
    {
        Invoke("PhaseEnd", 10f);
    }

    private void PhaseEnd()
    {
        BossManager.instance.PhaseEnd();
    }
}
