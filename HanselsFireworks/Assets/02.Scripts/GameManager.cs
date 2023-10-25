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
using Boss;

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

    private float sensitivity;
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
    
    public void SetSensitivity(float value)
    {
        sensitivity = value;
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
        if (BossManager.instance == null)
        {
            maxTime = UIManager.Instance.maxTime[currentStage];
            GetEnemies();
            currentBGM.clip = mainBGM;
            currentBGM.loop = true;
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
            // 각 페이즈에서는 UIManager가 씬에서 호출하지 않는다.
            // 그래서 BossManager에서 호출해야 한다. 
            maxTime = Boss.UIManager.Instance.maxTime[currentStage];
            currentBGM.clip = bossBgm;
            currentBGM.loop = true;
            mode = Mode.normal;
            leftTime = maxTime;
            leftMonster = 0;
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
    [SerializeField] KoreanSnack[] koreanSnacks;
    [SerializeField] Witch witch;

    // 각 씬에 유일하게 있는 SceneMgr이 호출 ㅋㅋㅋㅋㅋ
    public void GetKoreanSnacks()
    {
        koreanSnacks = FindObjectsOfType<KoreanSnack>();
    }
    // KoreanSnack 제어
    public void DeActivateKoreanSnacks()
    {        
        foreach (var item in koreanSnacks) item.DeActivate();        
    }

    // 각 씬에 유일하게 있는 SceneMgr이 호출 ㅋㅋㅋㅋㅋ
    public void GetWitch()
    {
        witch = FindObjectOfType<Witch>();
    }

    public void DeActivateWitch()
    {
        witch.DeActivate();
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
                    if (BossManager.instance == null) UIManager.Instance.ShowBonusUI();                    
                    else Boss.UIManager.Instance.ShowBonusUI();
                    AddBonusScore();
                }
            }
            else 
            {
                // SetEnemies(false);          // 의미 없음...
                if (BossManager.instance == null) DeActivateMonsters();     
                else
                {
                    if(BossManager.instance.currentPhase == 2)
                    {
                        DeActivateKoreanSnacks();       // 한국 과자 비활성화
                    }
                    else
                    {
                        DeActivateWitch();      // 마녀 또는 호박 비활성화.
                        PumkinManager.Instance.DeActivate();    // 호박 없애기
                    }
                }
                PlayWhistle();
                // 모든 플레이어, 적 이동 금지.  
                Debug.Log("End");
                StopCoroutine(Timer());
                stageScore[currentStage] = score;
                totalScore += stageScore[currentStage];
                if (BossManager.instance == null) 
                {
                    UIManager.Instance.PlayEnd();           // Make player don't move
                    UIManager.Instance.ShowResultUI();      // ShowResultUI
                    currentStage++;
                    score = 0;
                    SceneMgr.Instance.LoadNextScene();      // LoadNextScene
                }
                else 
                {
                    // 보스전에서 시간이 다 되면 BadEnding씬으로 넘어가기
                    // 여기서 Phase가 어디건 플레이어를 비활성화해야하는데...
                    // 페이즈 2 플레이어는 전혀 다른 놈이다... 
                    if(BossManager.instance.currentPhase == 2)
                    {
                        Boss.UIManager.Instance.DeActivatePlayer();
                    }
                    else
                    {
                        Boss.UIManager.Instance.PlayEnd();           // Make player don't move
                    }
                    Boss.UIManager.Instance.ShowResultUI();      // ShowResultUI
                    SceneMgr.Instance.LoadNextScene("07. BadEnding"); 
                    // SceneManager.LoadScene("07. BadEnding");
                }
                break;
            }
            yield return null;
        }
    }

    // BossManager에서 Phase3까지 완료시 호출
    public void BossObjective()
    {
        StopCoroutine(Timer());
        Boss.UIManager.Instance.ShowBonusUI();
        AddBonusScore();
        PlayWhistle();
        stageScore[currentStage] = score;
        totalScore += stageScore[currentStage];
        Boss.UIManager.Instance.PlayEnd();           // Make player don't move. 플레이어 사라짐
        Boss.UIManager.Instance.ShowResultUI();      // ShowResultUI
        SceneMgr.Instance.LoadNextScene("06. HappyEnding");          // 해피엔딩.
    }

}
