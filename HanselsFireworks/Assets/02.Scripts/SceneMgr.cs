using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float fakeLoadingTime = 2f; // ����ũ �ε� �ð� ���� (�� ����)

    void Update()
    {

    }

    public void LoadNextScene()
    {
        // �񵿱������� Scene�� �ҷ����� ���� Coroutine�� ����Ѵ�.
        StartCoroutine(LoadMyAsyncScene());
    }

    IEnumerator LoadMyAsyncScene()
    {
        // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        timer = 0;
        float fakeLoadingDuration = 1f / fakeLoadingTime; // ����ũ �ε� �ð��� ���� ���

        // Scene�� �ҷ����� ���� �Ϸ�� ������ ����Ѵ�.
        while (!asyncLoad.isDone)
        {
            // �����Ȳ Ȯ��
            if (asyncLoad.progress < 0.9f)
            {
                percent = asyncLoad.progress * 100f;
            }
            else
            {
                // 1�ʰ� ����ũ �ε�
                // ����ũ �ε�
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