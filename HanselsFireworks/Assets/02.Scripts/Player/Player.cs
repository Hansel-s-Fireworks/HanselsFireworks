using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Cursor;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    
    
    [SerializeField] Mode mode;
    public GameObject bullet;
    public Transform firePoint;

    [Header("Mouse Controll view")]
    public Transform characterBody;
    public Transform cameraTransform;
    public float mouseSensitivity = 7f;

    [Header("Debug")]
    public float mouseXInput;
    public float mouseYInput;
    private float xRotation = 0f;


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
        mode = GameManager.Instance.mode;
        // Cursor.lockState = CursorLockMode.Locked;       // 占쏙옙占쎌스 커占쏙옙占쏙옙 화占쏙옙효占?占쏙옙占?
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMode();
        PlayerView();        
    }

    private void UpdateMode()
    {
        mode = GameManager.Instance.mode;
        switch (mode)
        {
            case Mode.normal:
                if (Input.GetMouseButtonDown(0)) { Shoot(); }
                break;
            case Mode.Burst:
                if (Input.GetMouseButton(0))
                {
                    if (GameManager.Instance.leftCase <= 0)
                    {
                        GameManager.Instance.ChangeBGM();
                        GameManager.Instance.mode = Mode.normal;
                        break;
                    }
                    GameManager.Instance.leftCase -= 1;
                    //GameManager.Instance.tLeftCase.text = GameManager.Instance.leftCase.ToString();
                    Shoot();
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
    

    void Shoot()
    {
        // Instantiate(bullet, firePoint);
        // 메모리 풀을 이용해서 총알을 생성한다.
        GameObject clone = bulletMemoryPool.ActivatePoolItem();

        clone.transform.position = firePoint.position;
        clone.transform.rotation = firePoint.rotation;
        clone.GetComponent<PlayerBullet>().Setup(bulletMemoryPool);
    }

    

    public void TakeScore()
    {
        Debug.Log("Player Damaged");
        GameManager.Instance.combo = 1;     // 占쌨븝옙 占십깍옙화

        // 占쏙옙占?占쏙옙占쏙옙
        if (GameManager.Instance.score >= 100)
        {
            GameManager.Instance.score -= 100;     // 100占쏙옙 占쏙옙占쏙옙
        }
        else GameManager.Instance.score = 0;


        // GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
        // GameManager.Instance.tScore.text = GameManager.Instance.totalScore.ToString();
    }
}
