using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState 
{ 
    None = -1,
    Idle = 0,
    Pursuit, 
    Attack, 
    Hurt,
    Dead
}
public abstract class Enemy : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int score;

    public float delay;

    void Awake()
    {
        currentHP = maxHP;        
    }

    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;
        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;
                
        if (currentHP == 0) return true;
        return false;
    }
    public abstract void TakeScore();
    public abstract void TakeDamage(int damage);
}