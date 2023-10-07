using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float time;
    private MemoryPool memoryPool;

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
        transform.Translate(0, -speed * Time.fixedDeltaTime, 0);
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
            other.GetComponent<Player>().TakeDamage();
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
    }
}
