using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Translate(0, -speed * Time.deltaTime, 0);
        if (time > 3)
        {
            Destroy(this.gameObject);
        }
    }

    // 특정 방향으로 날아간다. 
    // public void Shoot(Vector3 position)
    // {
    //     StartCoroutine("OnMove", position);
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
