using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;


public class LongEnemy : Enemy
{
    [Header("Pursuit")]
    [SerializeField] private float attackRange;            // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRate = 1;

    public Animator animator;
    
    private Vector3 moveDirection = Vector3.zero;
    private EnemyState enemyState = EnemyState.None;    // 현재 적 행동
    private float lastAttackTime = 0;                   // 공격 주기 계산용 변수 

    [SerializeField] private Player target;                           // 적의 공격 대상(플레이어)

    public override void TakeDamage()
    {
        // bool isDead;
        Debug.Log("Enemy Damaged");
        gameObject.SetActive(false);
        WaveSpawner.Instance.leftMoster--;
        WaveSpawner.Instance.tLeftMonster.text = WaveSpawner.Instance.leftMoster.ToString();
        // 점수 추가
        // 남은 몬스터 수 줄기

        // Destroy(gameObject);        // 나중에 체력에 따른 제거 조건 다르게 하기
    }

    private void Start()
    {
        target = FindObjectOfType<Player>();        // 플레이어 인식
        animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= attackRange)        // 공격하기
        {
            animator.SetBool("Attack", true);
            ChangeState(EnemyState.Attack);
        }        
        else 
        { 
            animator.SetBool("Attack", false);
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // 현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기 때문에 return
        if (enemyState == newState) return;        
        StopCoroutine(enemyState.ToString());   // 이전에 재생중이던 상태 종료   
        enemyState = newState;                  // 현재 적의 상태를 newState로 설정        
        StartCoroutine(enemyState.ToString());  // 새로운 상태 재생
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            // 대기상태일 때, 하는 행동
            // 타겟과의 거리에 따라 행동 선태개(배회, 추격, 원거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            // 타겟과의 거리에 따라 행동 선택 (원거리 공격 / 정지)
            CalculateDistanceToTargetAndSelectState();
            if (Time.time - lastAttackTime > attackRate)
            {
                // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;

                // 발사체 생성
                Instantiate(projectilePrefab, projectileSpawnPoint.position,
                    projectileSpawnPoint.rotation);
            }
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {        
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // 내 위치        
        transform.rotation = Quaternion.LookRotation(to - from);            // 바로 돌기
    }
    private void MoveToTarget()
    {
        Vector3 to = target.transform.position; // 목표 위치
        Vector3 from = transform.position;      // 내 위치
        moveDirection = (to - from).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void OnDrawGizmos()
    {        
        // 목표 인식 및 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}