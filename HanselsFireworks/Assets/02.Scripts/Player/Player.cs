using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Cursor;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Rigidbody rb;
    
    public GameObject bullet;
    public Transform firePoint;
    public GameObject slowEffect;

    [Header("Mouse Controll view")]
    public Transform characterBody;
    public Transform cameraTransform;
    public float mouseSensitivity = 7f;

    [Header("Debug")]
    public float mouseXInput;
    public float mouseYInput;
    private float xRotation = 0f;


    // [SerializeField] private GameObject playerBullet;
    

    private MemoryPool bulletMemoryPool;
    

    private void Awake()
    {
        bulletMemoryPool = new MemoryPool(bullet);        
    }

    private void OnApplicationQuit()
    {
        bulletMemoryPool.DestroyObjects();
    }


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 3;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;       // ���콺 Ŀ���� ȭ��ȿ� ���
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { Shoot(); }
        PlayerView();        
    }
    private void FixedUpdate()
    {
        Move();
    }


    void PlayerView()
    {
        mouseXInput = Input.GetAxisRaw("Mouse X");
        mouseYInput = Input.GetAxisRaw("Mouse Y");
        Vector2 mouseDelta = new Vector2(mouseXInput * mouseSensitivity, mouseYInput * mouseSensitivity);

        xRotation -= mouseDelta.y;
        // ���Ʒ� �������� ���� ���� ����
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        characterBody.Rotate(Vector3.up * mouseDelta.x);
    }

    void Move()
    {
        // xz ���󿡼� ������ �Է�
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }

    void Shoot()
    {
        // Instantiate(bullet, firePoint);
        // �޸� Ǯ�� �̿��ؼ� �Ѿ��� �����Ѵ�. 
        GameObject clone = bulletMemoryPool.ActivatePoolItem();

        clone.transform.position = firePoint.position;
        clone.transform.rotation = firePoint.rotation;
        clone.GetComponent<PlayerBullet>().Setup(bulletMemoryPool);
    }

    

    public void TakeDamage()
    {
        Debug.Log("Player Damaged");
    }
}
