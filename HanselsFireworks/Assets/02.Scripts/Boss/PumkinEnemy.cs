using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinEnemy : Enemy
{
    private Animator animator;

    [SerializeField]
    private GameObject target;

    private float approachTime = 1f;

    private bool canTakeDamage = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsAttack", false);
    }

    private void Start()
    {
        animator.SetTrigger("IsAppear");
        PumkinManager.Instance.addPumkin(this.gameObject);

        target = GameObject.FindGameObjectWithTag("Player");
        canTakeDamage = true;
    }

    public override void TakeScore()
    {
        if (canTakeDamage)
        {
            GameManager.Instance.score += this.score * GameManager.Instance.combo;            
        }
    }

    public override void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            animator.SetTrigger("IsDamage");
            bool isDie = DecreaseHP(damage);
            if (isDie)
            {
                PumkinManager.Instance.DeletePumkin(this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        Debug.Log("Attack!");
        canTakeDamage = false;
        animator.SetTrigger("IsAttack");
        StartCoroutine(ApproachTarget());
    }

    IEnumerator ApproachTarget()
    {
        yield return new WaitForSeconds(1.0f);

        animator.enabled = false;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < approachTime)
        {
            Debug.Log("움직이는중");
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / approachTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PumkinManager.Instance.DeletePumkin(this.gameObject);
        Destroy(gameObject);
    }
}
