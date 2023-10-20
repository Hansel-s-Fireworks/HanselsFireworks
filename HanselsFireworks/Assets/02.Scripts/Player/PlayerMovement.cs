using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    private Vector3 moveForce;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk;
    [SerializeField] private AudioClip audioClipRun;
    [SerializeField] private AudioClip audioClipJump;

    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;

    private Rigidbody rb;
    bool isGrounded;
    bool isJump;
    public AudioSource audioSource;

    private void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
    void Jump()
    {
        if (Input.GetKeyDown(keyCodeJump) && !isJump)
        {
            // PlaySound(audioClipJump);
            print("점프");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {        
        bool isRun = false;
        isRun = Input.GetKey(keyCodeRun);
        float moveSpeed = isRun ? runSpeed : walkSpeed;
        if (isRun) PlaySound(audioClipRun); 
        else PlaySound(audioClipWalk);
        // xz 평면상에서 움직임 입력
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        moveForce = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
        rb.velocity = transform.rotation * moveForce;
    }
}
