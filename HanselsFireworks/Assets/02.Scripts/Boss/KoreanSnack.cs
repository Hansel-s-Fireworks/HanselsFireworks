using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoreanSnack : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결할 변수
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // 플레이어의 위치를 목표로 설정하여 몬스터가 따라가도록 합니다.
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
