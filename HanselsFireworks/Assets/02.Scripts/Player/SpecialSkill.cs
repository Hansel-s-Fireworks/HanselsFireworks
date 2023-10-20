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

    public AudioSource audioSource;

    void PlaySound(AudioClip clip)
    {
        audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
        audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
        audioSource.Play();             // 사운드 재생
    }
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
            doOnce = false;
            PlaySound(audioClipSkill);
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

        }
    }
}
