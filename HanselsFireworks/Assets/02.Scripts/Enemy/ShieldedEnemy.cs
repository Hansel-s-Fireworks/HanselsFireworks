using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShieldedEnemy : Enemy
{
    [Header("Move Speed")]
    public float pursuitSpeed;
    public float runSpeed;

    [Header("Info")]
    [SerializeField] private float attackRange;
    [SerializeField] private float recognitionRange;            // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk;
    [SerializeField] private AudioClip audioClipRun;
    [SerializeField] private AudioClip audioClipDie;
    [SerializeField] private AudioClip audioClipShieldBreak;
    [SerializeField] private AudioClip audioClipAttack;

    public int shieldScore;
    [SerializeField] private Player target;                           // 적의 공격 대상(플레이어)
    
    private Vector3 moveDirection = Vector3.zero;
    private EnemyState enemyState = EnemyState.None;    // 현재 적 행동
    public GameObject shield;
    NavMeshAgent nav;
    Rigidbody rb;
    
    public Animator animator;
    public BoxCollider candyCane;
    private CapsuleCollider collider;
    private NavMeshAgent navMeshAgent;
    private DissolveEnemy dissoveEffect;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>();        // 플레이어 인식
        animator = GetComponent<Animator>();
        animator.SetInteger("HP", currentHP);
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        dissoveEffect = GetComponent<DissolveEnemy>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ChangeState(EnemyState.Idle);
    }

    public override void TakeScore()
    {
        if (currentHP >= 1)
        {
            // 방패 점수
            GameManager.Instance.score += shieldScore * GameManager.Instance.combo;
            Debug.Log("Shielded_Gingerbread Damaged_1");
        }
        else if (currentHP == 0)
        {
            GameManager.Instance.score += this.score * GameManager.Instance.combo;
            Debug.Log("Shielded_Gingerbread Damaged_2");
        }
        
    }

    public override void TakeDamage(int damage)
    {
        // Debug.Log("Shielded_Gingerbread Damaged");
        bool isDie = DecreaseHP(damage);
        animator.SetInteger("HP", currentHP);
        nav.enabled = false;
        
        if (isDie == false)
        {
            PlaySound(audioClipShieldBreak);
            animator.SetTrigger("Hit");
            if (currentHP == 1)
            {
                shield.SetActive(false);
            }
        }
        else 
        {
            PlaySound(audioClipDie);

            GameManager.Instance.mode = Mode.Burst;
            GameManager.Instance.leftCase += 100;            
            GameManager.Instance.ChangeBGM();
            
            dissoveEffect.StartDissolve();      // 몬스터 효과 재생
            // 모든 코루틴 스탑 => 중간에 공격모션시 소리나는 에러때문에 
            StopAllCoroutines();
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            collider.enabled = false;
            navMeshAgent.enabled = false;       // 얘까지 꺼야 땅에 꺼진다. 

            // WaveSpawner.Instance.;
            Debug.Log("Shielded_Gingerbread Dead");
        }
    }

    

    
    void FreezeVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity= Vector3.zero;
    }
    private void FixedUpdate()
    {
        // FreezeVelocity();
    }

    private void SetStatebyDistance()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance < attackRange)
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Pursuit", false);
            ChangeState(EnemyState.Attack);
        }
        else if(distance > attackRange && distance <= recognitionRange)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Pursuit", true);
            ChangeState(EnemyState.Pursuit);
        }
        else if (distance > recognitionRange)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Pursuit", false);
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
        audioSource.Stop();
        nav.speed = 0;
        while (true)
        {
            // 대기상태일 때, 하는 행동
            // 타겟과의 거리에 따라 행동 선태개(배회, 추격, 원거리 공격)
            candyCane.enabled = false;
            nav.enabled = true;
            SetStatebyDistance();

            yield return null;
        }
    }

    private IEnumerator Pursuit()
    {
        PlaySound(audioClipRun);
        while (true)
        {
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            // MoveToTarget();                 // 타겟 방향을 계속 이동
            candyCane.enabled = false;
            nav.enabled = true;
            nav.speed = pursuitSpeed;
            nav.SetDestination(target.transform.position);
            SetStatebyDistance();
            yield return null;
        }
    }

    private IEnumerator Attack()
    {        
        while (true)
        {            
            nav.enabled = false;
            candyCane.enabled = true;
            FreezeVelocity();
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            animator.SetBool("Attack", true);
            PlaySound(audioClipAttack);
            SetStatebyDistance();
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // 내 위치        
        transform.rotation = Quaternion.LookRotation(to - from);            // 바로 돌기
    }

    // private void MoveToTarget()
    // {
    //     Vector3 to = target.transform.position; // 목표 위치
    //     Vector3 from = transform.position;      // 내 위치
    //     moveDirection = (to - from).normalized;
    //     transform.position += moveDirection * moveSpeed * Time.deltaTime;
    // }

    private void OnDrawGizmos()
    {
        // 목표 인식 및 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 추적 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, recognitionRange);
    }

    

}