using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumkin : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsAttack", false);

    }

    // Start is called before the first frame update
    private void Start()
    {
        animator.SetTrigger("IsAppear");
        Debug.Log(this.gameObject.name);
        PumkinManager.Instance.addPumkin(this.gameObject);
    }

    // 나중에 콜리전 처리로 바꿔야함
    public void GetDamage()
    {
        PumkinManager.Instance.DeletePumkin(this.gameObject);
        animator.SetTrigger("IsDamage");
    }

    public void PlayAttackAnimation()
    {
        if (PumkinManager.Instance.CheckPumkinExist(this.gameObject))
            animator.SetTrigger("IsAttack");
        else
            Destroy(this.gameObject);
    }    

}
