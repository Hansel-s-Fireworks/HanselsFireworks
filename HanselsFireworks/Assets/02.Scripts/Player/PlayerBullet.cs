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
        mode = GameManager.Instance.mode;       // �߻�� ���������� ��带 ����.
        // �׷��� start���� �̸� �����ϰ� ����.
        // �߻�ǰ� �븻�� �Ǿ��ٰ� �̹� �߻�Ȱ͵� �븻��� �Ѿ��� �ɼ��� ���⶧��. 
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
        if (time > 3)
        {
            // �̷��� ���� ������ �Ѿ��� �޺� �ʱ�ȭ�� �� ���̴�. 
            // ������ �Ѿ��� �Ӽ��� �ٲ���� �Ѵ�. 
            if (mode == Mode.normal)
            {
                Debug.Log("�޺� �ʱ�ȭ");
                GameManager.Instance.combo = 1; // �޺� �ʱ�ȭ
                GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
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
            // ����Ʈ �÷��� ������� �޸� Ǯ ����
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            if (mode == Mode.normal)
            {
                Debug.Log("�޺� �ʱ�ȭ");
                GameManager.Instance.combo = 1; // �޺� �ʱ�ȭ
                GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            }
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Interactable"))
        {
            Debug.Log("��ȣ�ۿ� ������Ʈ");
            other.GetComponent<InteractableObject>().TakeDamage(1);
            // �Ϲ� ������Ʈ�� �޺� ������ ����.
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        
    }
}