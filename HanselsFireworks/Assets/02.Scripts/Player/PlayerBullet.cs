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
        // 3�� �Ŀ��� �ı����� �ʾҴٸ� ���� ������ ���� ���̹Ƿ� �޺� �ʱ�ȭ
        if (time > 3 && !gameObject.IsDestroyed())
        {
            GameManager.Instance.combo = 1; // �޺� �ʱ�ȭ
            GameManager.Instance.tCombo.text = GameManager.Instance.combo.ToString();
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }
}