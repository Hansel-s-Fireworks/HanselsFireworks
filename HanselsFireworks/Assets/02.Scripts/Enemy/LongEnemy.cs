using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;


public class LongEnemy : Enemy
{
    [Header("Pursuit")]
    [SerializeField] private float attackRange;            // �ν� �� ���� ���� (�� ���� �ȿ� ������ Attack" ���·� ����)

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    public Animator animator;
    
    private Vector3 moveDirection = Vector3.zero;
    private EnemyState enemyState = EnemyState.None;    // ���� �� �ൿ
    // private float lastAttackTime = 0;                   // ���� �ֱ� ���� ���� 

    [SerializeField] private Player target;                           // ���� ���� ���(�÷��̾�)

    public override void TakeScore()
    {
        GameManager.Instance.totalScore += this.score * GameManager.Instance.combo;
        GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
    }

    public override void TakeDamage(int damage)
    {
        // bool isDead;
        Debug.Log("LongEnemy Damaged");
        bool isDie = DecreaseHP(damage);
        if(isDie)
        {
            gameObject.SetActive(false);                // ��Ȱ��ȭ
            GameManager.Instance.leftMonster--;         // ���� ���� �� �ٱ�
            GameManager.Instance.tLeftMonster.text = GameManager.Instance.leftMonster.ToString();
            Debug.Log("Shielded_Gingerbread Dead");
        }
    }

    private void Start()
    {
        target = FindObjectOfType<Player>();        // �÷��̾� �ν�
        animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= attackRange)        // �����ϱ�
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
        // ���� ������� ���¿� �ٲٷ��� �ϴ� ���°� ������ �ٲ� �ʿ䰡 ���� ������ return
        if (enemyState == newState) return;        
        StopCoroutine(enemyState.ToString());   // ������ ������̴� ���� ����   
        enemyState = newState;                  // ���� ���� ���¸� newState�� ����        
        StartCoroutine(enemyState.ToString());  // ���ο� ���� ���
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            // �������� ��, �ϴ� �ൿ
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���°�(��ȸ, �߰�, ���Ÿ� ����)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    // �ִϸ��̼� �̺�Ʈ�� ����
    private void ThrowCandyball()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position,
                    projectileSpawnPoint.rotation);
    }

    private IEnumerator Attack()
    {
        // LookRotationToTarget();
        while (true)
        {
            LookRotationToTarget();         // Ÿ�� ������ ��� �ֽ�
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���� (���Ÿ� ���� / ����)
            CalculateDistanceToTargetAndSelectState();
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
    }
}