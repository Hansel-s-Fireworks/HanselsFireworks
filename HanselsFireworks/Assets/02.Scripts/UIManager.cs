using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
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
    public Image stageInfo; 

    [Header("Result UI")]
    public GameObject resultPenel;
    public GameObject playPenel;
    public TextMeshProUGUI[] tStageScores;
    public TextMeshProUGUI tTotalScore;
    public TextMeshProUGUI tComboPlayScreen;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bonusSound;
    [SerializeField] private AudioClip sumResult;

    [SerializeField] private Animator animResultUI;
    [SerializeField] private Animator animPlayUI;
    [SerializeField] private float LoadingTime;
    [SerializeField] private Sprite[] stageImages;
    private int initTotalScore;
    private Player player;
    private AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftTime.text = GameManager.Instance.LeftTime.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        
        animResultUI = resultPenel.GetComponent<Animator>();
        animPlayUI = playPenel.GetComponent<Animator>();

        player = FindAnyObjectByType<Player>();
        
        GameManager.Instance.Init();
        
        initTotalScore = GameManager.Instance.totalScore;
        audioSource = GetComponent<AudioSource>();
        // ���콺 Ŀ���� ���̰� �ϰ� ����� �����մϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        InitInfo();
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
    void SetComponentEnabled<T>(bool isEnabled) where T : MonoBehaviour
    {
        MonoBehaviour componentToDisable = player.GetComponent<T>();
        componentToDisable.enabled = isEnabled;
    }

    public void PlayStart()
    {
        // ������Ʈ Ȱ��ȭ 
        SetComponentEnabled<Player>(true);
        SetComponentEnabled<PlayerMovement>(true);
        SetComponentEnabled<SpecialSkill>(true);
    }

    public void ShowResultUI()
    {
        InitScore();
        print("UI");
        animResultUI.SetBool("End", true);

        StartCoroutine(ShowResult());
    }

    public void ShowBonusUI()
    {
        tComboPlayScreen.text = GameManager.Instance.combo.ToString();
        animPlayUI.SetBool("KillAllMonster", true);
        audioSource.clip = bonusSound;
        audioSource.Play();
    }

    // public IEnumerator ShowBonusScore()
    // {
    // 
    // }


    // ��� �ʱ� ���� ����
    public void InitScore()
    {
        for (int i = 0; i < tStageScores.Length; i++)
        {
            tStageScores[i].text = GameManager.Instance.stageScore[i].ToString();
        }
        // �ʱ� ��� ���� 
        tTotalScore.text = initTotalScore.ToString();
    }
    public void InitInfo()
    {
        int curStage = GameManager.Instance.currentStage;
        stageInfo.sprite = stageImages[curStage];
    }

    IEnumerator ShowResult()
    {
        int curStage = GameManager.Instance.currentStage;
        int currentStageScore = GameManager.Instance.stageScore[curStage];
        int totalScore = GameManager.Instance.totalScore;
        yield return StartCoroutine(AnimateStageScore(0, currentStageScore));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(AnimateTotalScore(initTotalScore, totalScore));
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
        audioSource.clip = sumResult;
        audioSource.Play();
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



    // ���� ����
    public void StartUI()
    {
        // 3,2,1 ����. �̰� �ڷ�ƾ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.SetTimer();
        GameManager.Instance.SetObjective();
    }

}
