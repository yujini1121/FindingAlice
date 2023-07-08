using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject SettingBtn;
    [SerializeField] private GameObject Setting;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        DataController.instance.LoadData();
    }
    
    public void PlayerDead()
    {
        AsyncLoading.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSetting()
    {
        //GraphicsSettings.renderPipelineAsset = blurPipeline;
        Time.timeScale = 0;
        SettingBtn.SetActive(false);
        Setting.SetActive(true);
    }

    public void Continue()
    {
        //GraphicsSettings.renderPipelineAsset = defaultPipeline;
        Time.timeScale = 1;
        Setting.SetActive(false);
        SettingBtn.SetActive(true);
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
