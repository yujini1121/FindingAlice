using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DataController;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject settingBtn;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject joystick;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        DataController.instance.LoadSavePoint();

        if (DataController.instance.joystickFixed)
        {
            joystick.GetComponent<FixedJoystick>().enabled = true;
        }
        else
        {
            joystick.GetComponent<FloatingJoystick>().enabled = true;
        }
    }
    
    public void PlayerDead()
    {
        AsyncLoading.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSetting()
    {
        //GraphicsSettings.renderPipelineAsset = blurPipeline;
        Time.timeScale = 0;
        settingBtn.SetActive(false);
        setting.SetActive(true);
    }

    public void Continue()
    {
        //GraphicsSettings.renderPipelineAsset = defaultPipeline;
        Time.timeScale = 1;
        setting.SetActive(false);
        settingBtn.SetActive(true);
    }

    public void ExitGame()
    {
        AsyncLoading.LoadScene("ChapterSelect");
        //GraphicsSettings.renderPipelineAsset = defaultPipeline;
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
