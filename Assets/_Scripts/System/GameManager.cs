using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// ===================================================================================================
// 게임 속 플레이어의 죽음 처리 및 씬 전환, UI 관리
// ===================================================================================================

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public GameObject settingBtn;
    public GameObject setting;
    public GameObject joystick;
    public GameObject dialogue;

    [SerializeField] private GameObject settingBGSound;
    [SerializeField] private GameObject settingFXSound;

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

    void Start()
    {
        // 배경음 슬라이더 조작 시 슬라이더 값에 따라 음량을 조절하기 위한 델리게이트 지정
        Slider BGSound_Slider = settingBGSound.GetComponent<Slider>();
        BGSound_Slider.onValueChanged.AddListener(delegate
        {
            DataController.instance.bgSoundValue
                = settingBGSound.GetComponent<Slider>().value;
        });

        // 효과음 슬라이더 조작 시 슬라이더 값에 따라 음량을 조절하기 위한 델리게이트 지정
        Slider FXSound_Slider = settingFXSound.GetComponent<Slider>();
        FXSound_Slider.onValueChanged.AddListener(delegate
        {
            DataController.instance.fxSoundValue
                = settingFXSound.GetComponent<Slider>().value;
        });
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

        settingBGSound.GetComponent<Slider>().value = DataController.instance.bgSoundValue;
        settingFXSound.GetComponent<Slider>().value = DataController.instance.fxSoundValue;
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

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            OpenSetting();
        }
    }
}