using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShortEnemy : Enemy
{
    [Header("Move Speed")]
    public float pursuitSpeed;
    public float runSpeed;

    [Header("Info")]
    [SerializeField] private float attackRange;
    [SerializeField] private float recognitionRange;            // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)


    [SerializeField] private Player target;

    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk;
    [SerializeField] private AudioClip audioClipRun;
    [SerializeField] private AudioClip audioClipDie;
    [SerializeField] private AudioClip audioClipAttack;

    private Vector3 moveDirection = Vector3.zero;
    private EnemyState enemyState = EnemyState.None;
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
        GameManager.Instance.score += this.score * GameManager.Instance.combo;
        
    }

    


    public override void TakeDamage(int damage)
    {
        Debug.Log("Short_Gingerbread Damaged");
        bool isDie = DecreaseHP(damage);
        animator.SetInteger("HP", currentHP);
        nav.enabled = false;

        if (isDie)
        {
            PlaySound(audioClipDie);
            // animator.SetTrigger("Hit");
            dissoveEffect.StartDissolve();          // 몬스터 효과 재생
            // 모든 코루틴 스탑 => 중간에 공격모션시 소리나는 에러때문에 
            StopAllCoroutines();
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            // collider.enabled = false;
            // navMeshAgent.enabled = false;       // 얘까지 꺼야 땅에 꺼진다. 
            GameManager.Instance.leftMonster--;         // 남은 몬스터 수 줄기

            Debug.Log("Short_Gingerbread Dead");
        }
    }
    public void DeActivate()
    {
        StopAllCoroutines();
        audioSource.mute = true;
        navMeshAgent.enabled = false;
        animator.speed = 0;
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

    private void Update()
    {
    }

    private void SetStatebyDistance()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance < attackRange)
        {
            animator.SetBool("Pursuit", false);
            ChangeState(EnemyState.Attack);
        }
        else if(distance > attackRange && distance <= recognitionRange)
        {
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
            nav.enabled = true;
            candyCane.enabled = false;
            SetStatebyDistance();

            yield return null;
        }
    }

    private IEnumerator Pursuit()
    {
        PlaySound(audioClipRun);

        while (true)
        {
            animator.SetBool("Attack", false);
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            // MoveToTarget();                 // 타겟 방향을 계속 이동
            nav.enabled = true;
            candyCane.enabled = false;
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
            LookRotationToTarget();                 // 타겟 방향을 계속 주시
            animator.SetBool("Attack", true);
            PlaySound(audioClipAttack);
            SetStatebyDistance();
            yield return new WaitForSeconds(0.75f);
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);              // 내 위치  
        transform.rotation = Quaternion.LookRotation(to - from);                                // 바로 돌기
    }

    // private void MoveToTarget()
    // {
    //     Vector3 to = target.transform.position; // 목표 위치
    //     Vector3 from = transform.position;      
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