using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakableCookie : InteractableObject
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipBreak;

    private void Start()
    {
        // audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        // audioSource = gameObject.GetComponentInParent<AudioSource>();
    }

    public override void TakeScore()
    {
        GameManager.Instance.score += this.score;
    }

    public override void TakeDamage(int damage)
    {
        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            PlaySound(audioClipBreak);
            gameObject.SetActive(false);                // 비활성화
            GetComponent<BreakFruit>().Run();
            Debug.Log("BreakableCookie Breaked");
        }
    }
}