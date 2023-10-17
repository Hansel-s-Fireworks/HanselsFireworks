using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakableCookie : InteractableObject
{
    public override void TakeScore()
    {
        GameManager.Instance.score += this.score;
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