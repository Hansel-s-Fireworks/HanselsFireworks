using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Boss;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    [SerializeField]
    private GameObject sceneMng, canHideUI, aimUI;

    private bool playerCanAttack;
    public bool isSuccess2Phase;
    public int currentPhase;
    public UnityEvent<int> PhaseStartEvent;
    public UnityEvent<bool> PhaseEndEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
                Destroy(this.gameObject);
        }

        currentPhase = 1;
        playerCanAttack = false;
        isSuccess2Phase = false;
    }

    public void startBoss()
    {
        // GameManager.Instance.MuteBGM();
        PhaseStartEvent.Invoke(currentPhase);
    }

    public void PhaseEnd()
    {
        switch(currentPhase)
        {
            case 1:
                // Boss.UIManager.Instance.GetPlayer();        // 플레이어 얻기
                if (PumkinManager.Instance.GetPumkinList().Count == 0)
                {
                    Debug.Log("Phase1 Clear");
                    playerCanAttack = true;
                }
                else
                {
                    Debug.Log("Phase1 fail");
                    PhaseStartEvent.Invoke(1);
                    playerCanAttack = false;
                }
                PhaseEndEvent.Invoke(playerCanAttack);
                break;
            case 2:
                // Boss.UIManager.Instance.GetCharactorController();
                if (Phase2Manager.Instance.snackCnt == -1)
                {
                    Debug.Log("Phase2 Clear");
                    isSuccess2Phase = true;
                }
                else
                {
                    aimUI.SetActive(false);
                    Debug.Log("Phase2 fail");
                    isSuccess2Phase = false;
                }
                break;
            case 3:
                // Boss.UIManager.Instance.GetPlayer();        // 플레이어 얻기
                if (PumkinManager.Instance.GetPumkinList().Count == 0)
                {
                    Debug.Log("Phase3 Clear");
                    //sceneMng.GetComponent<SceneMgr>().nextSceneName = "06. HappyEnding";
                    playerCanAttack = true;
                }
                else
                {
                    Debug.Log("Phase3 fail");
                    //sceneMng.GetComponent<SceneMgr>().nextSceneName = "07. BadEnding";
                    PhaseStartEvent.Invoke(3);
                    playerCanAttack = false;
                }
                PhaseEndEvent.Invoke(playerCanAttack);
                break;
        }
    }

    public void goToNextPhase()
    {
        switch(currentPhase)
        {
            case 1:
                currentPhase++;
                SceneManager.LoadScene("Boss_Phase2");
                canHideUI.SetActive(false);
                aimUI.SetActive(false);
                break;
            case 2:
                currentPhase++;
                SceneManager.LoadScene("MAIN (JOOHONG ver) 4");
                canHideUI.SetActive(true);
                PhaseStartEvent.Invoke(currentPhase);
                break;
            case 3:
                // 결과창 띄우기
                // 근데 GM에서 관리하던건데 여기서 결과창 띄울 수 있나...
                // 코루틴이라.. 음
                // SceneManager.LoadScene("06. HappyEnding");
                GameManager.Instance.BossObjective();
                break;
        }    
    }

}
