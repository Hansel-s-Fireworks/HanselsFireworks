using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        // speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Translate(0, -speed * Time.deltaTime, 0);
        // 3초 후에도 파괴되지 않았다면 적을 맞추지 못한 것이므로 콤보 초기화
        if (time > 3 && !gameObject.IsDestroyed())
        {
            WaveSpawner.Instance.combo = 1; // 콤보 초기화
            WaveSpawner.Instance.tCombo.text = WaveSpawner.Instance.combo.ToString();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            other.GetComponent<Enemy>().TakeScore();
            WaveSpawner.Instance.combo++;
            WaveSpawner.Instance.tCombo.text = WaveSpawner.Instance.combo.ToString();
            Destroy(gameObject);
        }
    }
}