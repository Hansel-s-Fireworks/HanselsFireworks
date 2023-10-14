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

    public int[] stageScore;
    public int score;
    public int totalScore;
    public int currentStage;
    public int maxTime;
    


    private int leftTime { get; set; }
    public int LeftTime { get { return leftTime; } set { leftTime = value; } }
    public int leftMonster;
    public int combo;
    public int leftCase;
    public Mode mode;

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
    public void Init() 
    {        
        leftTime = maxTime;
        leftMonster = 0;
        score = 0;
        combo = 1;
        leftCase = 0;
        isMonsterLeft = true;
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
    private int bounsScore = 500;
    private bool isMonsterLeft;

    public void AddBonusScore()
    {
        score += bounsScore * combo;
        // totalScore += bounsScore * combo;
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

                    // stageScore[currentStage] = score;
                    // totalScore += stageScore[currentStage];
                    // UIManager.Instance.ShowResultUI();      // ShowResultUI
                    // 
                    // currentStage++;
                    // score = 0;
                    // SceneMgr.Instance.LoadNextScene();      // LoadNextScene
                    // break;
                }
            }
            else 
            {
                // 완전 끝
                // 모든 플레이어, 적 이동 금지. 
                Debug.Log("End");
                StopCoroutine(Timer());
                stageScore[currentStage] = score;
                totalScore += stageScore[currentStage];
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
