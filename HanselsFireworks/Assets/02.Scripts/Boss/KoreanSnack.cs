using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoreanSnack : Enemy
{
    public Transform player; // ?�레?�어??Transform???�결??변??
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
        // ?�레?�어???�치�?목표�??�정?�여 몬스?��? ?�라가?�록 ?�니??
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
