using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] myPumkins;
    
    public void PrepareAttack()
    {
        StartCoroutine(SpawnPumkinWithDelay());
    }

    public void Attack()
    {

    }

    IEnumerator SpawnPumkinWithDelay()
    {
        foreach (GameObject obj in myPumkins)
        {
            yield return new WaitForSeconds(0.7f); // 1초 대기

            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
