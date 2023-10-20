using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimatorController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public float MoveSpeed
    {
        set => animator.SetFloat("movementSpeed", value);
        get => animator.GetFloat("movementSpeed");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    // 매개변수로 받아온 애니메이션이 현재 재생중인지 확인 후 bool 반환  
    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void SetFloat(string paramName, float value)
    {
        animator.SetFloat(paramName, value);
    }
}
