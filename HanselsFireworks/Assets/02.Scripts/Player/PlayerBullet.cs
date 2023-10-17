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
        mode = GameManager.Instance.mode;       // 占쌩삼옙占?占쏙옙占쏙옙占쏙옙占쏙옙占쏙옙 占쏙옙躍?占쏙옙占쏙옙.
        // 占쌓뤄옙占쏙옙 start占쏙옙占쏙옙 占싱몌옙 占쏙옙占쏙옙占싹곤옙 占쏙옙占쏙옙.
        // 占쌩삼옙품占?占쎈말占쏙옙 占실억옙占쌕곤옙 占싱뱄옙 占쌩삼옙활孤占?占쎈말占쏙옙占?占싼억옙占쏙옙 占심쇽옙占쏙옙 占쏙옙占썩때占쏙옙. 
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
        // 3占쏙옙 占식울옙占쏙옙 占식깍옙占쏙옙占쏙옙 占십았다몌옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占싱므뤄옙 占쌨븝옙 占십깍옙화
        if (time > 3)
        {
            // 占싱뤄옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占싼억옙占쏙옙 占쌨븝옙 占십깍옙화占쏙옙 占쏙옙 占쏙옙占싱댐옙. 
            // 占쏙옙占쏙옙占쏙옙 占싼억옙占쏙옙 占쌈쇽옙占쏙옙 占쌕뀐옙占쏙옙占?占싼댐옙. 
            if (mode == Mode.normal)
            {
                Debug.Log("占쌨븝옙 占십깍옙화");
                GameManager.Instance.combo = 1; // 占쌨븝옙 占십깍옙화
                // GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
            time = 0;       // 占시곤옙占쏙옙 占쌕쏙옙 0占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙占쏙옙 占쌕쏙옙 占쏙옙占쏙옙占싫댐옙.
                            // 占쌓뤄옙占쏙옙 占쏙옙占쏙옙占쏙옙 if占쏙옙占쏙옙 占쏙옙占쏙옙 占쌕뤄옙 占쏙옙활占쏙옙화占싫댐옙. 
            memoryPool.DeactivatePoolItem(gameObject);      // 占쏙옙활占쏙옙화
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
            // GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            // 占시뤄옙占싱억옙 占쏙옙크占쏙옙트占쏙옙 占쌍댐옙 Impact
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            // bool 占쏙옙占쏙옙 占싹놂옙 占쏙옙화占쌍몌옙 占싸몌옙 占쏙옙크占쏙옙트占쏙옙占쏙옙 占쌨몌옙 풀 占쏙옙占쏙옙
            // 占쏙옙占쏙옙트 占시뤄옙占쏙옙 占쏙옙占쏙옙占쏙옙占?占쌨몌옙 풀 占쏙옙占쏙옙
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            if (mode == Mode.normal)
            {
                Debug.Log("占쌨븝옙 占십깍옙화");
                GameManager.Instance.combo = 1; // 占쌨븝옙 占십깍옙화
                // GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Interactable"))
        {
            Debug.Log("占쏙옙호占쌜울옙 占쏙옙占쏙옙占쏙옙트");
            other.GetComponent<InteractableObject>().TakeDamage(1);
            // 占싹뱄옙 占쏙옙占쏙옙占쏙옙트占쏙옙 占쌨븝옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙.
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        
    }
}