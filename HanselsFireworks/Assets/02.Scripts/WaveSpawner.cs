using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    private static WaveSpawner instance = null;
    public static WaveSpawner Instance 
    { 
        get 
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        } 
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject enemy;
    public float startTime;
    public float endTime;
    public float spawnRate;
    public int leftTime;
    public int leftMoster;
    [Header("UI")]
    public TextMeshProUGUI tLeftTime;
    public TextMeshProUGUI tLeftMonster;

    public Vector3 spawnRange;


    // Start is called before the first frame update
    void Start()
    {
        tLeftTime.text = leftTime.ToString();
        InvokeRepeating("Spawn", 0, spawnRate);
        StartCoroutine(Timer());
        StartCoroutine(CheckObjective());
        Invoke("CancelInvoke", endTime);
    }

    void Spawn()
    {
        Vector3 spawnArea = new Vector3(
            Random.Range(transform.position.x - spawnRange.x, transform.position.x + spawnRange.x),
            transform.position.y, 
            Random.Range(transform.position.z - spawnRange.z, transform.position.z + spawnRange.z));
        Vector3 spawnRotate = new Vector3(0, Random.Range(0, 180), 0);
        Instantiate(enemy, spawnArea, Quaternion.Euler(spawnRotate));

        leftMoster++;
        tLeftMonster.text = leftMoster.ToString();
    }

    IEnumerator Timer()
    {
        yield return new WaitUntil(() => leftMoster >= 20);
        while (leftTime > 0)
        {
            leftTime -= 1;
            tLeftTime.text = leftTime.ToString();
            if(leftMoster == 0) break;
            yield return new WaitForSeconds(1f);
        }
        print("Timer coroutine end");
    }

    IEnumerator CheckObjective()
    {
        yield return new WaitUntil(() => leftMoster >= 20);
        while (true)
        {
            if (leftTime > 0)
            {
                if (leftMoster == 0)
                {
                    Debug.Log("Win");
                    break;
                }
            }
            else 
            { 
                Debug.Log("Lose"); 
                break;
            }
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        // 기즈모 색상 지정
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, spawnRange * 2);
    }

}
