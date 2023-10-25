using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float time;
    private MemoryPool memoryPool;
    public Vector3 moveDirection = Vector3.zero;        // 적이 방향을 전달

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
    }

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        // transform.Translate(0, -speed * Time.fixedDeltaTime, 0);
        transform.position += moveDirection * speed * Time.fixedDeltaTime;
        if (time > 3)
        {
            time = 0;
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeScore();
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if(other.CompareTag("Floor"))
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
