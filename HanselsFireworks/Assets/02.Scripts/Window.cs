using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : InteractableObject
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipBreak;

    [SerializeField] private AudioSource ccc;

    private void Awake()
    {
        // ccc = GetComponentInParent<AudioSource>();
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
            // PlaySound(audioClipBreak);
            ccc.Play();
            gameObject.SetActive(false);                // 비활성화
            GetComponent<BreakFruit>().Run();
            Debug.Log("Window Breaked");
        }
    }
}
