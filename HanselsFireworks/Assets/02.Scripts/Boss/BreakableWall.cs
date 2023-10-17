using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : Enemy
{
    [SerializeField]
    public AudioSource damageSound;

    private void Start()
    {
        maxHP = 2;
        currentHP = 2;
    }

    public override void TakeScore()
    {
    }

    public override void TakeDamage(int damage)
    {
        damageSound.Play();

        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            gameObject.SetActive(false);
        }
    }
}
