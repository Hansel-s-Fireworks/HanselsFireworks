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
    // public GameObject startPenel;
    public TextMeshProUGUI[] tStageScores;
    public TextMeshProUGUI tTotalScore;

    [SerializeField] private Animator animResultUI;
    [SerializeField] private Animator animStartUI;

    [SerializeField] private float LoadingTime;

    // Start is called before the first frame update
    void Start()
    {
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftTime.text = GameManager.Instance.LeftTime.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        animResultUI = resultPenel.GetComponent<Animator>();
        GameManager.Instance.Init();
        // animStartUI = startPenel.GetComponent<Animator>();
        // 마우스 커서를 보이게 하고 잠금을 해제합니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        tLeftTime.text = GameManager.Instance.LeftTime.ToString();
        tLeftMonster.text = GameManager.Instance.leftMonster.ToString();
        tScore.text = GameManager.Instance.score.ToString();
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     ShowResultUI();
        // }
    }


    public void ShowResultUI()
    {
        InitScore();
        print("UI");
        animResultUI.SetBool("End", true);

        StartCoroutine(ShowResult());
    }


    // 결과 초기 점수 갱신
    public void InitScore()
    {
        for (int i = 0; i < tStageScores.Length; i++)
        {
            tStageScores[i].text = GameManager.Instance.stageScore[i].ToString();
        }
        tTotalScore.text = GameManager.Instance.totalScore.ToString();
    }

    IEnumerator ShowResult()
    {
        int curStage = GameManager.Instance.currentStage;
        int currentStageScore = GameManager.Instance.stageScore[curStage];
        int totalScore = GameManager.Instance.totalScore;
        yield return StartCoroutine(AnimateStageScore(0, currentStageScore));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(AnimateTotalScore(currentStageScore, totalScore));
    }

    IEnumerator AnimateStageScore(int a, int b)
    {
        int curStage = GameManager.Instance.currentStage;
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / LoadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;

            tempScore = (int)Mathf.Lerp(a, b, temp);

            tStageScores[curStage].text = tempScore.ToString();
            if (tempScore >= b)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator AnimateTotalScore(int a, int b)
    {
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / LoadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;

            tempScore = (int)Mathf.Lerp(a, b, temp);

            tTotalScore.text = tempScore.ToString();
            if (tempScore >= b)
            {
                break;
            }
            yield return null;
        }
    }



    // 게임 시작
    public void StartUI()
    {
        // 3,2,1 실행. 이거 코루틴
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.SetTimer();
        GameManager.Instance.SetObjective();
    }

}
