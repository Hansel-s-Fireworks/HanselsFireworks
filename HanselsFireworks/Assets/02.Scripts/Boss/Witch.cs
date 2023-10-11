using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{
    [SerializeField]
    private GameObject[] myPumkins;
    [SerializeField]
    private GameObject pumkinPrefab;

    private bool canDamage = false;
    private bool isAttacking = false;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 시작하는 조건 추가해야함
    public void PrepareAttack()
    {
        StartCoroutine(SpawnPumkinWithDelay());
    }

    public void MyTurn(bool canTakeDamage)
    {
        canDamage = canTakeDamage;
        if (!canTakeDamage)
        {
            Debug.Log("마녀가 공격");
            // 마녀 공격 애니메이션
            animator.SetTrigger("IsAttacking");
            PumkinManager.Instance.Attack();
        }
        else
        {
            Debug.Log("내가 공격");
            canDamage = true;
            // 맞는 애니메이션
        }
    }

    public override void TakeScore()
    {
        if (canDamage)
        {
            GameManager.Instance.totalScore += this.score * GameManager.Instance.combo;
            GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
        }
    }

    public override void TakeDamage(int damage)
    {
        if (canDamage)
        {
            animator.SetTrigger("IsDamage");
            bool isDie = DecreaseHP(damage);
            if (isDie)
            {
                canDamage = false;
                Debug.Log("1페이즈 끝");
                animator.SetTrigger("IsDead");
            }
        }
    }

    IEnumerator SpawnPumkinWithDelay()
    {
        foreach (GameObject obj in myPumkins)
        {
            yield return new WaitForSeconds(0.7f);

            if (obj != null)
            {
                Instantiate(pumkinPrefab, obj.transform);

                obj.SetActive(true);
            }
        }
    }
}
