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

    // Start is called before the first frame update
    void Start()
    {
        impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
        transform.SetParent(null);
        // speed = 10;
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
        // 3�� �Ŀ��� �ı����� �ʾҴٸ� ���� ������ ���� ���̹Ƿ� �޺� �ʱ�ȭ
        if (time > 3 && !gameObject.IsDestroyed())
        {
            GameManager.Instance.combo = 1; // �޺� �ʱ�ȭ
            GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            time = 0;       // �ð��� �ٽ� 0���� �������� �ٽ� �����ȴ�.
                            // �׷��� ������ if���� ���� �ٷ� ��Ȱ��ȭ�ȴ�. 
            memoryPool.DeactivatePoolItem(gameObject);      // ��Ȱ��ȭ
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
            // �÷��̾� ��ũ��Ʈ�� �ִ� Impact
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            // bool ���� �ϳ� ��ȭ�ָ� �θ� ��ũ��Ʈ���� �޸� Ǯ ����
            // ����Ʈ �÷��� ������ �޸� Ǯ ����
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        
    }
}