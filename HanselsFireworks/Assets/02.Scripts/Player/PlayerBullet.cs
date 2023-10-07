using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public float time;
    private MemoryPool memoryPool;
    private ImpactMemoryPool impactMemoryPool;
    [SerializeField] private Mode mode;
    // Start is called before the first frame update
    void Start()
    {
        impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
        transform.SetParent(null);
        
        // speed = 10;
    }
    private void OnEnable()
    {
        mode = GameManager.Instance.mode;       // 발사된 시점에서의 모드를 저장.
        // 그래서 start에서 미리 저장하고 진행.
        // 발사되고 노말이 되었다고 이미 발사된것도 노말모드 총알이 될수는 없기때문. 
    }
    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.Translate(0, -speed * Time.fixedDeltaTime, 0);
        // 3초 후에도 파괴되지 않았다면 적을 맞추지 못한 것이므로 콤보 초기화
        if (time > 3)
        {
            // 이러면 나간 시점의 총알이 콤보 초기화가 될 것이다. 
            // 생성된 총알의 속성을 바꿔줘야 한다. 
            if (mode == Mode.normal)
            {
                Debug.Log("콤보 초기화");
                GameManager.Instance.combo = 1; // 콤보 초기화
                GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
            time = 0;       // 시간을 다시 0으로 만들어줘야 다시 생성된다.
                            // 그렇지 않으면 if문에 의해 바로 비활성화된다. 
            memoryPool.DeactivatePoolItem(gameObject);      // 비활성화
            // Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {            
            other.GetComponent<Enemy>().TakeDamage(1);
            other.GetComponent<Enemy>().TakeScore();
            GameManager.Instance.combo++;
            GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            // 플레이어 스크립트에 있는 Impact
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            // bool 변수 하나 변화주면 부모 스크립트에서 메모리 풀 실행
            // 이펙트 플레이 끝나고서 메모리 풀 해제
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            if (mode == Mode.normal)
            {
                Debug.Log("콤보 초기화");
                GameManager.Instance.combo = 1; // 콤보 초기화
                GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        
    }
}