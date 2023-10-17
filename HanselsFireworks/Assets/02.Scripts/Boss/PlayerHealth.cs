using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth, currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 충돌한 오브젝트가 Enemy 레이어에 속한다면 플레이어의 체력을 감소시킴
            currentHealth -= 10; // 원하는 만큼 감소시킵니다.

            if (currentHealth <= 0)
            {
                BossManager.instance.goToNextPhase();
                BossManager.instance.PhaseEnd();
            }
        }
    }
}
