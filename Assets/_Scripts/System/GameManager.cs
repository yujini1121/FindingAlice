using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject settingBtn;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject joystick;
    [SerializeField] public GameObject dialogue;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DataController.instance.LoadSavePoint();

        if (DataController.instance.joystickFixed)
        {
            Destroy(joystick.GetComponent<FloatingJoystick>());
        }
        else
        {
            Destroy(joystick.GetComponent<FixedJoystick>());
        }
    }
    
    public void PlayerDead()
    {
        AsyncLoading.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSetting()
    {
        Time.timeScale = 0;
        settingBtn.SetActive(false);
        setting.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        setting.SetActive(false);
        settingBtn.SetActive(true);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        AsyncLoading.LoadScene("ChapterSelect");
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //    {
    //        isPaused = true;
    //        PopUpOption();
    //    }
    //    if (isPaused)
    //    {
    //        isPaused = false;
    //    }
    //}
}
