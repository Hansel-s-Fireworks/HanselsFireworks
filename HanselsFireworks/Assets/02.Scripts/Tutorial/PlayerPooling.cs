using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{

    public class PlayerPooling : MonoBehaviour
    {
        public Transform firePoint;

        [Header("Mouse Controll view")]
        public Transform characterBody;
        public Transform cameraTransform;
        public float mouseSensitivity;

        [Header("Debug")]
        public float mouseXInput;
        public float mouseYInput;
        private float xRotation = 0f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip audioClipHurt;
        [SerializeField] private Animator gunAnimator;

        public AudioSource audioSource;
        public FireGunWithPooling fireGun;


        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            PlayerView();
            // if (Input.GetMouseButton(0))
            // {
            //     fireGun.StartWeaponAction();
            // }

            if (Input.GetMouseButtonDown(0))
            {
                fireGun.isAutomaticAttack = true;
                fireGun.StartWeaponAction();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                fireGun.StopWeaponAction();
            }
        }

        void PlaySound(AudioClip clip)
        {
            audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
            audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
            audioSource.Play();             // 사운드 재생
        }
        void PlayerView()
        {
            mouseXInput = Input.GetAxisRaw("Mouse X");
            mouseYInput = Input.GetAxisRaw("Mouse Y");
            Vector2 mouseDelta = new Vector2(mouseXInput * mouseSensitivity, mouseYInput * mouseSensitivity);

            xRotation -= mouseDelta.y;
            // 위아래 내려보는 각도 범위 제한
            xRotation = Mathf.Clamp(xRotation, -90f, 80f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            characterBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }

}