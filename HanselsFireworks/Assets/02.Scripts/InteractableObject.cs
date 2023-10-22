using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int score;
    [SerializeField] protected AudioSource audioSource;

    void Awake()
    {
        currentHP = maxHP;
    }

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
        audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
        audioSource.Play();             // 사운드 재생
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
