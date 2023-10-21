using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    public float walkSpeed;
    // public float runSpeed;
    // public float jumpForce;
    private Vector3 moveForce;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk;

    // [Header("Input KeyCodes")]
    // [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;
    // [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;

    private Rigidbody rb;
    bool isGrounded;
    bool isJump;
    public AudioSource audioSource;

    private void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody>();
        // audioSource = GetComponent<AudioSource>();
    }   

    void PlaySound(AudioClip clip)
    {
        audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
        audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
        audioSource.Play();             // 사운드 재생
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
    /*void Jump()
    {
        if (Input.GetKeyDown(keyCodeJump) && !isJump)
        {
            // PlaySound(audioClipJump);
            print("점프");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJump = true;
        }
    }*/

    private void Update()
    {
        // Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중일떄
        if (x != 0 || z != 0)
        {
            // bool isRun = false;
            // 옆이나 뒤로 이동시 달리기 제한
            // if (z > 0) isRun = Input.GetKey(keyCodeRun);
            // 
            // moveSpeed = isRun ? runSpeed : walkSpeed;
            // audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;
            moveSpeed = walkSpeed;
            audioSource.clip = audioClipWalk;
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        // 제자리에 멈춰있을 때
        else
        {
            moveSpeed = 0;
            // 멈췄을때 사운드가 재생 중이면 정지
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        // xz 평면상에서 움직임 입력
        Vector2 targetVelocity = new Vector2(x * moveSpeed, z * moveSpeed);
        moveForce = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
        rb.velocity = transform.rotation * moveForce;
    }
}
