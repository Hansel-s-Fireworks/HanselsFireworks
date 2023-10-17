using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Enemy
{
    [SerializeField]
    private AudioSource damageSound;

    public override void TakeScore()
    {
    }

    public override void TakeDamage(int damage)
    {
        damageSound.Play();
    }
}
