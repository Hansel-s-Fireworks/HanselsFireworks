using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    [SerializeField]
    private GameObject sceneMng, canHideUI;

    private bool playerCanAttack;
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
    }

    public void startBoss()
    {
        GameManager.Instance.MuteBGM();
        PhaseStartEvent.Invoke(currentPhase);
    }

    public void PhaseEnd()
    {
        switch(currentPhase)
        {
            case 1:
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
                if (Phase2Manager.Instance.snackCnt == -1)
                {
                    Debug.Log("Phase2 Clear");
                }
                else
                {
                    Debug.Log("Phase2 fail");
                }
                break;
            case 3:
                if (PumkinManager.Instance.GetPumkinList().Count == 0)
                {
                    Debug.Log("Phase3 Clear");
                    sceneMng.GetComponent<SceneMgr>().nextSceneName = "06. HappyEnding";
                    playerCanAttack = true;
                }
                else
                {
                    Debug.Log("Phase3 fail");
                    sceneMng.GetComponent<SceneMgr>().nextSceneName = "07. BadEnding";
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
                break;
            case 2:
                currentPhase++;
                SceneManager.LoadScene("Boss");
                canHideUI.SetActive(true);
                if (Phase2Manager.Instance.snackCnt == -1)
                {
                    Debug.Log("어지러워");
                }
                PhaseStartEvent.Invoke(currentPhase);
                break;
            case 3:
                SceneManager.LoadScene("06. HappyEnding");
                break;
        }    
    }

}
