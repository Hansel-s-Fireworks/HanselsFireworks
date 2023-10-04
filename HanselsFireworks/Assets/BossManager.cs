using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
    public UnityEvent OnFailEvent;

    private void Start()
    {
        Invoke("CheckPumkins", 15f);
    }

    private void CheckPumkins()
    {
        if (PumkinManager.Instance.GetPumkinList().Count == 0)
            Debug.Log("Clear");
        else
        {
            Debug.Log("fail");
            OnFailEvent.Invoke();
        }
    }

}
