using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject aimSensitivityPanel;
        [SerializeField] private Slider sensitivity;
        [SerializeField] private TextMeshProUGUI tSensitivity;


        [SerializeField] private Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Tutorial.Player>();
        }

        // Update is called once per frame
        void Update()
        {
            if (mainPanel.activeSelf == false)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ActivateCursor();
                    OnActivePlayer(false);
                    aimSensitivityPanel.SetActive(true);
                }
            }
            if (aimSensitivityPanel.activeSelf)
            {
                SetSensitivity();
            }
        }


        void SetComponentEnabled<T>(bool isEnabled) where T : MonoBehaviour
        {
            MonoBehaviour componentToDisable = player.GetComponent<T>();
            componentToDisable.enabled = isEnabled;
        }

        public void PlayStart()
        {
            // 컴포넌트 활성화 
            SetComponentEnabled<Player>(true);
            SetComponentEnabled<PlayerMovement>(true);
            // SetComponentEnabled<SpecialSkill>(true);
        }

        public void OnActivePlayer(bool active)
        {
            SetComponentEnabled<Player>(active);
            SetComponentEnabled<PlayerMovement>(active);
        }


        public void ActivateCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DeActivateCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void SetSensitivity()
        {
            player.mouseSensitivity = sensitivity.value;
            tSensitivity.text = sensitivity.value.ToString();
            PlayerPrefs.SetFloat("Sensitive", sensitivity.value);
            // GameManager.Instance.SetSensitivity(sensitivity.value);
        }

    }

}