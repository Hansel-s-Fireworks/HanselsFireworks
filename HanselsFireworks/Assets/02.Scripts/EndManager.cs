using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public GameManager gameManager;
    public BossManager bossManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameManager = FindObjectOfType<GameManager>();
        bossManager = FindObjectOfType<BossManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        Destroy(gameManager.gameObject);
        Destroy(bossManager.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnHome()
    {
        // SceneManager.LoadScene("01.StartMenu");
    }

}
