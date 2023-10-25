using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    public GameObject debuff;
    // Start is called before the first frame update
    void Start()
    {
        if (!BossManager.instance.isSuccess2Phase)
        {
            debuff.SetActive(true);
        }
    }

}
