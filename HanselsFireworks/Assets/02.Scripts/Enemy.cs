using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { None = -1, Idle = 0, Pursuit, Attack, Dead}
public abstract class Enemy : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField]
    protected float moveSpeed;

    [Header("HP")]
    [SerializeField]
    protected int maxHP = 100;
    protected int currentHP;

    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        // delay = 5;
        // Destroy(gameObject, delay);
    }


    public abstract void TakeDamage();
}