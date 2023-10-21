using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoreanSnack : Enemy
{
    public Transform player; // ?뚮젅?댁뼱??Transform???곌껐??蹂??
    private NavMeshAgent navMeshAgent;

    [SerializeField] private AudioClip audioClipDie;

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
        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            gameObject.SetActive(false);
            Phase2Manager.Instance.snackCnt--;
        }
    }

    private void Update()
    {
        // ?뚮젅?댁뼱???꾩튂瑜?紐⑺몴濡??ㅼ젙?섏뿬 紐ъ뒪?곌? ?곕씪媛?꾨줉 ?⑸땲??
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
