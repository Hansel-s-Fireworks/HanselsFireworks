using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour
{
    private BreakableCookie[] breakableCookies;
    private Enemy[] enemies;
    private ParticleSystem smellEffects;
    [SerializeField] bool doOnce;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipSkill;

    public GameObject skillEffect;
    public AudioSource audioSource;


    void PlaySound(AudioClip clip)
    {
        audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
        audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
        audioSource.Play();             // 사운드 재생
    }

    void PlayEffect()
    {
        StartCoroutine(ScaleUpEffect(0,0.5f));
    }

    [SerializeField] private float loadingTime;
    IEnumerator ScaleUpEffect(float a, float b)
    {
        float temp = 0;
        float LoadingDuration = 1f / loadingTime;
        Vector3 scale = new Vector3(0,0,0.05f);
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;
            scale.x = Mathf.Lerp(a, b, temp);
            scale.y = Mathf.Lerp(a, b, temp);       // 0.2f
            scale.z = Mathf.Lerp(0.05f, 0.14f, temp);
            skillEffect.transform.localScale = scale;

            if (scale.x >= b)
            {
                break;
            }
            yield return null;
        }
    }


    void ReturnTime() => Time.timeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        doOnce = true;
        // audioSource = GetComponent<AudioSource>();
        breakableCookies = FindObjectsOfType<BreakableCookie>();
        enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && doOnce)
        {
            Time.timeScale = 0.1f;
            UIManager.Instance.specialSkillIcon.color = new Color(0.4f, 0.4f, 0.4f, 1);
            doOnce = false;
            PlayEffect();
            PlaySound(audioClipSkill);
            Invoke("ReturnTime", 0.2f);
            foreach (var item in breakableCookies)
            {
                smellEffects = item.GetComponentInChildren<ParticleSystem>();
                smellEffects.Play();
            }
            foreach (var item in enemies)
            {
                smellEffects = item.GetComponentInChildren<ParticleSystem>();
                smellEffects.Play();
            }
            // 화면상에 Effect

        }
    }
}
