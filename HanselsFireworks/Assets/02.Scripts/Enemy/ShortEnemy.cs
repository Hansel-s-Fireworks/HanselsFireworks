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
    [SerializeField] private float recognitionRange;            // �ν� �� ���� ���� (�� ���� �ȿ� ������ Attack" ���·� ����)
    

    [SerializeField] private Player target;                           // ���� ���� ���(�÷��̾�)
    
    private Vector3 moveDirection = Vector3.zero;
    private EnemyState enemyState = EnemyState.None;    // ���� �� �ൿ
    public GameObject shield;
    NavMeshAgent nav;
    Rigidbody rb;
    
    public Animator animator;

    public override void TakeScore()
    {
        GameManager.Instance.totalScore += this.score * GameManager.Instance.combo;
        GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Shielded_Gingerbread Damaged");
        bool isDie = DecreaseHP(damage);
        animator.SetInteger("HP", currentHP);
        nav.enabled = false;
        
        if (isDie == false)
        {
            animator.SetTrigger("Hit");
            if (currentHP > 1)
            {
                // ChangeState(EnemyState.Hurt);
                // animator.SetTrigger("Hit");
            }
            else if (currentHP == 1)
            {
                // ChangeState(EnemyState.Hurt);
                // animator.SetTrigger("Shield Crash");
                shield.SetActive(false);
            }
        }
        else 
        {
            // ChangeState(EnemyState.Dead);
            // animator.Play("Dead");
            gameObject.SetActive(false);
            // WaveSpawner.Instance.;
            Debug.Log("Shielded_Gingerbread Dead");
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>();        // �÷��̾� �ν�
        animator = GetComponent<Animator>();
        animator.SetInteger("HP", currentHP);
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        ChangeState(EnemyState.Idle);
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
        // ���� ������� ���¿� �ٲٷ��� �ϴ� ���°� ������ �ٲ� �ʿ䰡 ���� ������ return
        if (enemyState == newState) return;
        StopCoroutine(enemyState.ToString());   // ������ ������̴� ���� ����   
        enemyState = newState;                  // ���� ���� ���¸� newState�� ����        
        StartCoroutine(enemyState.ToString());  // ���ο� ���� ���
    }

    private IEnumerator Idle()
    {
        nav.speed = 0;
        while (true)
        {
            // �������� ��, �ϴ� �ൿ
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���°�(��ȸ, �߰�, ���Ÿ� ����)
            nav.enabled = true;
            SetStatebyDistance();

            yield return null;
        }
    }

    private IEnumerator Pursuit()
    {
        while (true)
        {
            LookRotationToTarget();         // Ÿ�� ������ ��� �ֽ�
            // MoveToTarget();                 // Ÿ�� ������ ��� �̵�
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
            FreezeVelocity();
            LookRotationToTarget();         // Ÿ�� ������ ��� �ֽ�
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���� (���Ÿ� ���� / ����)
            SetStatebyDistance();
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // ��ǥ ��ġ
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // �� ��ġ        
        transform.rotation = Quaternion.LookRotation(to - from);            // �ٷ� ����
    }

    // private void MoveToTarget()
    // {
    //     Vector3 to = target.transform.position; // ��ǥ ��ġ
    //     Vector3 from = transform.position;      // �� ��ġ
    //     moveDirection = (to - from).normalized;
    //     transform.position += moveDirection * moveSpeed * Time.deltaTime;
    // }

    private void OnDrawGizmos()
    {
        // ��ǥ �ν� �� ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ���� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, recognitionRange);
    }

    

}