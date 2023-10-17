using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum Mode
{
    NULL = -1,
    normal,
    Burst
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance 
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

    // public GameObject enemy;
    public float startTime;
    public float endTime;
    public float spawnRate;
    public int leftTime;
    public int leftMonster;
    public int totalScore;
    public int combo;
    public int leftCase;
    public Mode mode;

    [Header("UI")]
    public TextMeshProUGUI tLeftTime;
    public TextMeshProUGUI tLeftMonster;
    public TextMeshProUGUI tScore;
    public TextMeshProUGUI tCombo;
    public TextMeshProUGUI tLeftCase;

    public Vector3 spawnRange;


    // Start is called before the first frame update
    void Start()
    {
        leftMonster = 0;
        totalScore = 0;
        combo = 1;
        leftCase = 0;
        tCombo.text = combo.ToString();
        tLeftTime.text = leftTime.ToString();
        tLeftCase.text = leftCase.ToString();
        // InvokeRepeating("Spawn", 0, spawnRate);
        StartCoroutine(Timer());
        StartCoroutine(CheckObjective());
        // Invoke("CancelInvoke", endTime);
    }
    private void Update()
    {
        leftMonster = GetLeftEnemies();
        tLeftMonster.text = leftMonster.ToString();
    }


    void Spawn()
    {
        Vector3 spawnArea = new Vector3(
            Random.Range(transform.position.x - spawnRange.x, transform.position.x + spawnRange.x),
            transform.position.y, 
            Random.Range(transform.position.z - spawnRange.z, transform.position.z + spawnRange.z));
        Vector3 spawnRotate = new Vector3(0, Random.Range(0, 180), 0);
        // Instantiate(enemy, spawnArea, Quaternion.Euler(spawnRotate));

        leftMonster++;
        tLeftMonster.text = leftMonster.ToString();
    }

    public int GetLeftEnemies()
    {
        // ��� Enemy ������Ʈ�� ���� ���� ������Ʈ �迭�� ã���ϴ�.
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Enemy ������Ʈ�� ���� ���� ������Ʈ�� ������ ��ȯ�մϴ�.
        return enemies.Length;
    }

    public void SetScore()
    {
        
    }

    IEnumerator Timer()
    {
        yield return new WaitUntil(() => leftMonster >= 1);
        while (leftTime > 0)
        {
            leftTime -= 1;
            tLeftTime.text = leftTime.ToString();
            if(leftMonster == 0) break;
            yield return new WaitForSeconds(1f);
        }
        print("Timer coroutine end");
    }

    IEnumerator CheckObjective()
    {
        yield return new WaitUntil(() => leftMonster >= 1);
        while (true)
        {
            if (leftTime > 0)
            {
                if (leftMonster == 0)
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
        // ����� ���� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, spawnRange * 2);
    }

}
