using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakableCookie : Enemy
{
    public override void TakeScore()
    {
        GameManager.Instance.totalScore += this.score * GameManager.Instance.combo;
        GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
    }

    public override void TakeDamage(int damage)
    {
        // bool isDead;
        Debug.Log("LongEnemy Damaged");
        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            gameObject.SetActive(false);                // ��Ȱ��ȭ
            GameManager.Instance.leftMonster--;         // ���� ���� �� �ٱ�
            GameManager.Instance.tLeftMonster.text = GameManager.Instance.leftMonster.ToString();
            GetComponent<BreakFruit>().Run();
            Debug.Log("BreakableCookie Dead");
        }
    }
}