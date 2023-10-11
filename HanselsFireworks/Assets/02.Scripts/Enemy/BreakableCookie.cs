using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakableCookie : InteractableObject
{
    public override void TakeScore()
    {
        GameManager.Instance.totalScore += this.score;
        GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
    }

    public override void TakeDamage(int damage)
    {
        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            gameObject.SetActive(false);                // ��Ȱ��ȭ
            GetComponent<BreakFruit>().Run();
            Debug.Log("BreakableCookie Breaked");
        }
    }
}