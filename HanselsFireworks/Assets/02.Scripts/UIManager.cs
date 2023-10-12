using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("UI")]
    public TextMeshProUGUI tLeftTime;
    public TextMeshProUGUI tLeftMonster;
    public TextMeshProUGUI tScore;
    public TextMeshProUGUI tCombo;
    public TextMeshProUGUI tLeftCase;

    [Header("Result UI")]
    public GameObject resultPenel;
    public TextMeshProUGUI[] tStageScores;
    public TextMeshProUGUI tTotalScore;

    [SerializeField] private Animator uiAnimator;

    [SerializeField] private float LoadingTime;

    // Start is called before the first frame update
    void Start()
    {
        LoadingTime = 2;
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftTime.text = GameManager.Instance.leftTime.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        uiAnimator = resultPenel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        tLeftTime.text = GameManager.Instance.leftTime.ToString();
        tLeftMonster.text = GameManager.Instance.leftMonster.ToString();
        tScore.text = GameManager.Instance.score.ToString();
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ShowResultUI();
        }
    }


    public void ShowResultUI()
    {
        print("UI");
        uiAnimator.SetBool("End", true);
        
        StartCoroutine(AnimateScore(GameManager.Instance.currentStage));
    }

    IEnumerator AnimateScore(int curStage)
    {
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / LoadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;
            
            tempScore = (int)Mathf.Lerp(0, GameManager.Instance.stageScore[curStage], temp);

            tStageScores[curStage].text = tempScore.ToString();
            if (tempScore >= GameManager.Instance.stageScore[curStage])
            {
                break;
            }
            yield return null;
        }
    }

    // 전체 점수 코루틴
    IEnumerator AnimateScore()
    {
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / LoadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;

            tempScore = (int)Mathf.Lerp(GameManager.Instance.totalScore, GameManager.Instance.totalScore + GameManager.Instance.score, temp);

            tTotalScore.text = tempScore.ToString();
            if (tempScore >= GameManager.Instance.totalScore + GameManager.Instance.score)
            {
                break;
            }
            yield return null;
        }
    }

    public void StartUI()
    {
        // 3,2,1 실행. 이거 코루틴
    }

}
