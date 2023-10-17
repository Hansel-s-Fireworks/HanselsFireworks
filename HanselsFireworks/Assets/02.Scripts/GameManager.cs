﻿using System.Collections;
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

    [SerializeField]
    private AudioSource mainBGM, burstBGM, currentBGM;


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
        // 紐⑤뱺 Enemy 而댄룷?뚰듃瑜?媛吏?寃뚯엫 ?ㅻ툕?앺듃 諛곗뿴??李얠뒿?덈떎.
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Enemy 而댄룷?뚰듃瑜?媛吏?寃뚯엫 ?ㅻ툕?앺듃??媛쒖닔瑜?諛섑솚?⑸땲??
        return enemies.Length;
    }

    public void SetScore()
    {
        stageScore[0] = score;
    }
    public void Init() 
    {        
        if (BossManager.instance == null)
        {
            Debug.Log("보스매니저 존재안함");
            leftTime = maxTime;
            leftMonster = 0;
            score = 0;
            combo = 1;
            leftCase = 0;
            isMonsterLeft = true;
        }
        else
        {
            leftTime = 90;
            leftMonster = 1;
            score = 0;
            combo = 1;
            leftCase = 0;
            isMonsterLeft = true;
            Debug.Log("보스매니저 존재");
        }
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

    public void ChangeBGM()
    {
        if (currentBGM == burstBGM)
        {
            burstBGM.mute = true;
            mainBGM.mute = false;
            currentBGM = mainBGM;
        }
        else
        {
            burstBGM.mute = false;
            mainBGM.mute = true;
            currentBGM = burstBGM;
        }
        currentBGM.Play();
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
                // ?꾩쟾 ??
                // 紐⑤뱺 ?뚮젅?댁뼱, ???대룞 湲덉?. 
                Debug.Log("End");
                StopCoroutine(Timer());
                stageScore[currentStage] = score;
                totalScore += stageScore[currentStage];
                UIManager.Instance.ShowResultUI();      // ShowResultUI
                
                currentStage++;
                score = 0;
                SceneMgr.Instance.LoadNextScene();      // LoadNextScene
                //bgm.mute = true;
                break;
            }
            yield return null;
        }
    }



}
