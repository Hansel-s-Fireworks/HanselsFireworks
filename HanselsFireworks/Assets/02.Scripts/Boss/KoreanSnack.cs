using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoreanSnack : Enemy
{
    public Transform player; // ?Œë ˆ?´ì–´??Transform???°ê²°??ë³€??
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private AudioSource damageSound;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            Phase2Manager.Instance.snackCnt--;
        }
    }

    private void Update()
    {
        // ?Œë ˆ?´ì–´???„ì¹˜ë¥?ëª©í‘œë¡??¤ì •?˜ì—¬ ëª¬ìŠ¤?°ê? ?°ë¼ê°€?„ë¡ ?©ë‹ˆ??
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
