using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2Bullet : MonoBehaviour
{
    public float speed;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.Translate(0, -speed * Time.fixedDeltaTime, 0);

        if(time > 3)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Interactable"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            // other.GetComponent<Enemy>().TakeScore();
            Destroy(gameObject);
        }

    }
}
