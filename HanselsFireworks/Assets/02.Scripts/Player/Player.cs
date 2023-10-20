using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Cursor;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class Player : MonoBehaviour
{   
    [SerializeField] Mode mode;
    public Transform firePoint;

    [Header("Mouse Controll view")]
    public Transform characterBody;
    public Transform cameraTransform;
    public float mouseSensitivity = 7f;

    [Header("Debug")]
    public float mouseXInput;
    public float mouseYInput;
    private float xRotation = 0f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipHurt;
    public AudioSource audioSource;


    [SerializeField] private Animator gunAnimator;
    public FireGun fireGun;


    // Start is called before the first frame update
    void Start()
    {
        mode = GameManager.Instance.mode;
        // audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {        
        UpdateMode();
        PlayerView();        
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
        audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
        audioSource.Play();             // 사운드 재생
    }
    private void UpdateMode()
    {
        mode = GameManager.Instance.mode;
            switch (mode)
            {
                case Mode.normal:
                    if (Input.GetMouseButtonDown(0))
                    {
                        fireGun.StartWeaponAction();
                    }
                    break;
                case Mode.Burst:
                    if (Input.GetMouseButtonDown(0))
                    {
                        fireGun.isAutomaticAttack = true;
                        fireGun.StartWeaponAction();
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        fireGun.StopWeaponAction();
                    }
                break;
                default:
                    break;
            }
        
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

    public void TakeScore()
    {
        Debug.Log("Player Damaged");
        PlaySound(audioClipHurt);
        // audioSource.clip = audioClipHurt;
        // audioSource.loop = true;
        // audioSource.Play();
        
        GameManager.Instance.combo = 1;     // 콤보 초기화

        // 양수 유지
        if (GameManager.Instance.score >= 100)
        {
            GameManager.Instance.score -= 100;     // 100점 감점
        }
        else GameManager.Instance.score = 0;
    }
}
