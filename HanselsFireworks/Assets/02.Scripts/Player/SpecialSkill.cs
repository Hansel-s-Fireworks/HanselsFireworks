using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour
{
    private BreakableCookie[] breakableCookies;
    private ParticleSystem smellEffects;
    bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        doOnce = true;
        breakableCookies = FindObjectsOfType<BreakableCookie>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && doOnce)
        {
            doOnce = false;
            foreach (var item in breakableCookies)
            {
                smellEffects = item.GetComponentInChildren<ParticleSystem>();
                smellEffects.Play();
            }
        }
    }
}
