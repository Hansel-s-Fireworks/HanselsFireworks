using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Boss;

public class SceneMgr : MonoBehaviour
{
    private static SceneMgr instance;
    public static SceneMgr Instance
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

    public string nextSceneName;
    [Range(0, 100)] public float percent;
    public float timer;
    public float fakeLoadingTime; // 페이크 로딩 시간 설정 (초 단위)
    private bool isLoading = false;


    private void Start()
    {
        if (BossManager.instance != null)
        {
            if (BossManager.instance.currentPhase == 2)
            {
                GameManager.Instance.GetKoreanSnacks();
                Boss.UIManager.Instance.GetPhaseTwoPlayer();
            }
            else
            {
                GameManager.Instance.GetWitch();
                Boss.UIManager.Instance.GetPlayer();           // Make player don't move
            }
        }
    }
    void Update()
    {

    }

    public void LoadNextScene(string sceneName)
    {
        nextSceneName = sceneName;
        if (!isLoading)
        {
            isLoading = true;
            // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
            StartCoroutine(LoadMyAsyncScene());
        }
    }

    public void LoadNextScene()
    {
        if (nextSceneName == "") return;
        if (!isLoading)
        {
            isLoading = true;
            // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
            StartCoroutine(LoadMyAsyncScene());
        }
    }

    IEnumerator LoadMyAsyncScene()
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        timer = 0;
        float fakeLoadingDuration = 1f / fakeLoadingTime; // 페이크 로딩 시간의 역수 계산

        // Scene을 불러오는 것이 완료될 때까지 대기한다.
        while (!asyncLoad.isDone)
        {
            // 진행상황 확인
            if (asyncLoad.progress < 0.9f)
            {
                percent = asyncLoad.progress * 100f;
            }
            else
            {
                // 1초간 페이크 로딩
                // 페이크 로딩
                timer += Time.deltaTime * fakeLoadingDuration;
                percent = Mathf.Lerp(90f, 100f, timer);
                if (percent >= 100)
                {
                    asyncLoad.allowSceneActivation = true;
                    yield break;
                }
            }
            yield return null;
        }
    }
}