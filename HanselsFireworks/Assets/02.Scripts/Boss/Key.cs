using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private AudioSource gameClear;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("플레이어");
            gameClear.Play();
            Invoke("GoTo3Phase", 0.5f);
        }
    }

    private void GoTo3Phase()
    {
        BossManager.instance.PhaseEnd();
        BossManager.instance.goToNextPhase();
    }
}
