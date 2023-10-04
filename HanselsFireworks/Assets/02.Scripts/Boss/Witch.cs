using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] myPumkins;
    private bool canDamage = false;
    private bool isAttacking = false;
    private Animator animator;
    [SerializeField]
    private Transform targetTransform;  /// player Transform
    [SerializeField]
    private int hp;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAttacking)
        {
            isAttacking = false;

            // 플레이어 위치 받아와서 호박 Transform 변환
        }
    }

    // 시작하는 조건 추가해야함
    public void PrepareAttack()
    {
        StartCoroutine(SpawnPumkinWithDelay());
    }

    public void MyTurn(bool canAttack)
    {
        Debug.Log("마녀의 턴입니다");
        Debug.Log(canAttack);
        if (!canAttack)
        {
            Debug.Log("마녀가 공격");
            // 마녀 공격 애니메이션
            animator.SetTrigger("IsAttacking");
            PumkinManager.Instance.PlayAttackAnimatation();
            Invoke("MyPumkinAttack", 1f);
        }
        else
        {
            Debug.Log("내가 공격");
            canDamage = true;
            // 맞는 애니메이션
        }
    }

    public void GetDamage()
    {
        if (hp > 0)
        {
            hp--;
            Debug.Log("마녀의 남은 hp:" + hp);
            animator.SetTrigger("IsDamage");
        }
        else if (hp == 0)
        {
            animator.SetTrigger("IsDead");
        }
    }

    IEnumerator SpawnPumkinWithDelay()
    {
        foreach (GameObject obj in myPumkins)
        {
            yield return new WaitForSeconds(0.7f);

            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }



    private void MyPumkinAttack()
    {
        isAttacking = true;
    }
}
