using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class FireGunNonPooling : MonoBehaviour
    {
        [Header("Weapon Setting")]
        public GameObject bullet;
        public float attackRate = 0.1f;            // 공격 속도
        public bool isAutomaticAttack;      // 연속 공격 여부
        [SerializeField] private Transform bulletSpawnPoint;             // 총알 생성 위치

        [Header("Audio Clips")]
        [SerializeField] private AudioClip audioClipFire;                // 공격 사운드
                                                                         // [SerializeField] private AudioClip burstBGM;

        GunAnimatorController animator;
        AudioSource audioSource;                // 사운드 재생 컴포넌트
        float lastAttackTime = 0;
        bool isAttack;                  // 공격 여부 체크용

        private void Start()
        {
            isAttack = false;
            isAutomaticAttack = false;
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<GunAnimatorController>();
        }
        private void PlaySound(AudioClip clip)
        {
            audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
            audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
            audioSource.Play();             // 사운드 재생
        }

        public void StartWeaponAction()
        {
            StartCoroutine("OnAttackLoop");
        }

        public void StopWeaponAction()
        {
            // 마우스 왼쪽 클릭 (공격 종료)
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }

        private IEnumerator OnAttackLoop()
        {
            while (true)
            {
                OnAttack();
                if (isAttack)
                {
                    isAutomaticAttack = false;
                    break;
                }
                yield return null;
            }
        }


        public void OnAttack()
        {
            if (Time.time - lastAttackTime > attackRate)
            {
                // if (animator.MoveSpeed > 0.5f) return;
                // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;

                // 무기 애니메이션 재생
                // 같은 애니메이션을 반복할 때, 애니메이션을 끊고 처음부터 다시 재생
                animator.Play("Fire", -1, 0);

                // 총구 이펙트 재생
                // StartCoroutine("OnMuzzleFlashEffect");
                Debug.Log("Shoot Gun Anim");
                GameObject clone = Instantiate(bullet, bulletSpawnPoint);

                // 공격 사운드 재생
                PlaySound(audioClipFire);
            }
        }
    }
}