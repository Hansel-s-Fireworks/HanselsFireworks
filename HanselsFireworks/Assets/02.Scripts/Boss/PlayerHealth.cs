using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth, currentHealth;

    [SerializeField]
    private List<GameObject> hearts;

    private int heartIndex;
    private bool canDamage;

    private void Start()
    {
        currentHealth = maxHealth;
        heartIndex = 3;
        canDamage = true;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canDamage && hit.gameObject.tag == "Enemy")
        {
            Debug.Log("충돌");
            currentHealth -= 10; // 원하는 만큼 감소시킵니다.

            canDamage = false;

            StartCoroutine(ChangeCanDamageAfterDelay(1.0f));

            if (heartIndex >= 0)
                hearts[heartIndex--].SetActive(false);

            if (currentHealth == 0)
            {
                BossManager.instance.isSuccess2Phase = false;
                Invoke("GoTo3Phase", 0.5f);
            }
        }
    }

    private IEnumerator ChangeCanDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1초 지연

        canDamage = true;
    }

    private void GoTo3Phase()
    {
        BossManager.instance.PhaseEnd();
        BossManager.instance.goToNextPhase();
    }
}
