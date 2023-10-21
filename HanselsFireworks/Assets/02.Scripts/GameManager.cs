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

// 한글 테스트
// 여기도되나용?
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

    public int[] stageScore;
    public int score;
    public int totalScore;
    public int currentStage;
    private int maxTime;

    [SerializeField] private AudioSource currentBGM;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainBGM;
    [SerializeField] private AudioClip burstBGM;
    [SerializeField] private AudioClip bossBgm;
    [SerializeField] private AudioClip resultBgm;

    private int leftTime { get; set; }
    public int LeftTime { get { return leftTime; } set { leftTime = value; } }
    public int leftMonster;
    public int combo;
    public int leftCase;
    public Mode mode;

    private int bounsScore = 500;
    private bool isMonsterLeft;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        stageScore = new int[3];
    }
    private void Update()
    {
        leftMonster = GetLeftEnemies();        
    }
    

    public int GetleftTime() { return leftTime; }

    public int GetLeftEnemies()
    {
        // 모든 Enemy 컴포넌트를 가진 게임 오브젝트 배열을 찾습니다.
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        
        // Enemy 컴포넌트를 가진 게임 오브젝트의 개수를 반환합니다.
        return enemies.Length;
    }

    public void SetScore()
    {
        stageScore[0] = score;
    }

    // UIManager가 매 씬전환 시작때마다 start함수에서 호출하는 초기화 함수
    public void Init() 
    {
        maxTime = UIManager.Instance.maxTime[currentStage];
        GetEnemies();
        currentBGM.clip = mainBGM;
        currentBGM.loop = true;
        // 시작 전 모든 적들 움직임 정지시키기 위해 테스트
        // SetEnemies(false);

        print("모든 적 코루틴 정지");

        if (BossManager.instance == null)
        {
            Debug.Log("보스매니저 존재안함");
            mode = Mode.normal;
            leftTime = maxTime;
            leftMonster = 0;
            score = 0;
            combo = 1;
            leftCase = 0;
            isMonsterLeft = true;            
        }
        else
        {
            mode = Mode.normal;
            leftTime = 90;
            leftMonster = 1;
            score = 0;
            combo = 1;
            leftCase = 0;
            isMonsterLeft = true;
            Debug.Log("보스매니저 존재");
        }
    }


    public ShortEnemy[] shortEnemies;
    public LongEnemy[] longEnemies;
    public ShieldedEnemy[] shieldedEnemies;
    public void GetEnemies()
    {
        shortEnemies = FindObjectsOfType<ShortEnemy>();
        longEnemies = FindObjectsOfType<LongEnemy>();
        shieldedEnemies = FindObjectsOfType<ShieldedEnemy>();
    }

    // 적들 제어
    public void SetEnemies(bool active)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var item in enemies)
        {
            item.enabled = active;
        }
    }
    
    public void DeActivateMonsters()
    {        
        foreach (var item in shortEnemies) item.DeActivate();
        foreach (var item in longEnemies) item.DeActivate();
        foreach (var item in shieldedEnemies) item.DeActivate();
    }

    public void SetTimer()
    {
        StartCoroutine(Timer());
    }
    public void SetObjective()
    {
        StartCoroutine(CheckObjective());
    }

    IEnumerator Timer()
    {
        yield return new WaitUntil(() => leftMonster >= 1);
        while (leftTime > 0)
        {
            leftTime -= 1;
            
            // if(leftMonster == 0) break;
            yield return new WaitForSeconds(1f);
        }
        print("Timer coroutine end");
    }

    public void AddBonusScore()
    {
        score += bounsScore * combo;
    }

    public void PlayMainBGM()
    {
        currentBGM.Stop();
        currentBGM.clip = mainBGM;
        currentBGM.Play();
    }
    public void PlayBurstBGM()
    {
        currentBGM.Stop();
        currentBGM.clip = burstBGM;
        currentBGM.Play();
    }
    public void PlayBossBGM()
    {
        currentBGM.Stop();
        currentBGM.clip = bossBgm;
        currentBGM.Play();
    }

    public void PlayWhistle()
    {
        if (currentBGM.loop == true)
        {
            currentBGM.loop = false;
            currentBGM.clip = resultBgm;
            currentBGM.Play();
        }
    }

    public void MuteBGM()
    {
        currentBGM.mute = true;
    }

    IEnumerator CheckObjective()
    {
        yield return new WaitUntil(() => leftMonster >= 1);
        while (true)
        {
            if (leftTime > 0)
            {
                if (leftMonster == 0 && isMonsterLeft)
                {
                    isMonsterLeft = false;
                    // bonus score
                    UIManager.Instance.ShowBonusUI();                    
                    AddBonusScore();
                }
            }
            else 
            {
                // SetEnemies(false);          // 의미 없음...
                DeActivateMonsters();       
                PlayWhistle();
                // 모든 플레이어, 적 이동 금지.  
                Debug.Log("End");
                StopCoroutine(Timer());
                stageScore[currentStage] = score;
                totalScore += stageScore[currentStage];
                UIManager.Instance.PlayEnd();           // Make player don't move
                UIManager.Instance.ShowResultUI();      // ShowResultUI
                
                currentStage++;
                score = 0;
                SceneMgr.Instance.LoadNextScene();      // LoadNextScene
                break;
            }
            yield return null;
        }
    }



}
