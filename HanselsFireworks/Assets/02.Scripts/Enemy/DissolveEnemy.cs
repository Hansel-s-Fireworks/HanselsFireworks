using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    List<Material> materials = new List<Material>();

    [Header("Dissolve Time")]
    [SerializeField] private float dissolveTime;
    // private float timer;
    private Animator animator;


    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
        animator = GetComponent<Animator>();
    }

    private void Reset()
    {
        Start();
        SetValue(0);
    }

    public void StartDissolve()
    {
        animator.speed = 0;     // 멈추기
        StartCoroutine(DissolveCookie());
    }

    IEnumerator DissolveCookie()
    {
        float timer = 0;
        float fakeLoadingDuration = 1f / dissolveTime;
        while (true)
        {
            timer += Time.deltaTime * fakeLoadingDuration;
            
            float value = Mathf.Lerp(0f, 1f, timer);
            SetValue(value);
            if(value >= 1)
            {
                gameObject.SetActive(false);        // 다 재생 후 활성화
                yield break;
            }
            yield return null;
        }
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
    }
}

