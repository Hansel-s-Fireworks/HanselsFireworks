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
            // �浹�� ������Ʈ�� Enemy ���̾ ���Ѵٸ� �÷��̾��� ü���� ���ҽ�Ŵ
            currentHealth -= 10; // ���ϴ� ��ŭ ���ҽ�ŵ�ϴ�.

            if (currentHealth <= 0)
            {
                BossManager.instance.goToNextPhase();
                BossManager.instance.PhaseEnd();
            }
        }
    }
}
