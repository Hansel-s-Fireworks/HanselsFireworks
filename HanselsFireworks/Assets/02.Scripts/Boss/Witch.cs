using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{
    [SerializeField]
    AudioSource  attackSound, laughingSound;
    [SerializeField]
    private GameObject[] myPumkins;
    [SerializeField]
    private GameObject pumkinPrefab, barrier;
    private bool canDamage = false;
    private bool isAttacking = false;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        BossManager.instance.PhaseStartEvent.AddListener(PrepareAttack);
        BossManager.instance.PhaseEndEvent.AddListener(MyTurn);

        if (BossManager.instance.currentPhase == 3)
        {
            PrepareAttack(3);
        }
    }

    // ?쒖옉?섎뒗 議곌굔 異붽??댁빞??
    public void PrepareAttack(int phase)
    {
        switch(phase)
        {
            case 1:
                foreach(GameObject pumkins in myPumkins)
                {
                    pumkins.GetComponent<PumkinAnimation>().angularSpeed = 40;
                }
                break;
            case 3:
                foreach (GameObject pumkins in myPumkins)
                {
                    pumkins.GetComponent<PumkinAnimation>().angularSpeed = 80;
                }
                break;
        }

        StartCoroutine(SpawnPumkinWithDelay());
        barrier.SetActive(true);
        laughingSound.Play();
    }

    public void MyTurn(bool canTakeDamage)
    {
        canDamage = canTakeDamage;
        if (!canTakeDamage)
        {
            animator.SetTrigger("IsAttacking");
            attackSound.Play();
            PumkinManager.Instance.Attack();
        }
        else
        {
            canDamage = true;
            barrier.SetActive(false);
        }
    }

    public override void TakeScore()
    {
        if (canDamage)
        {
            GameManager.Instance.score += this.score * GameManager.Instance.combo;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (canDamage)
        {
            animator.SetTrigger("IsDamage");
            bool isDie = DecreaseHP(damage);

            if (currentHP == maxHP - damage * 5)
            {
                Debug.Log("씬전환");
                BossManager.instance.goToNextPhase();                
            }    
            if (isDie)
            {
                canDamage = false;
                animator.SetTrigger("IsDead");
                // 마녀 비활성화
                // gameObject.SetActive(false);
            }
        }
    }

    public void DeActivate()
    {
        StopAllCoroutines();
        attackSound.mute = true;
        laughingSound.mute = true;
        animator.speed = 0;
    }

    IEnumerator SpawnPumkinWithDelay()
    {
        foreach (GameObject obj in myPumkins)
        {
            yield return new WaitForSeconds(0.7f);

            if (obj != null)
            {
                Instantiate(pumkinPrefab, obj.transform);

                obj.SetActive(true);
            }
        }
    }
}
