using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    [SerializeField] private GameObject chapters;
    [SerializeField] private GameObject chapterInfo;
    [SerializeField] private GameObject chapterInfoSprite;
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject settingBGSound;
    [SerializeField] private GameObject settingFXSound;
    [SerializeField] private GameObject settingFixedJoystick;
    private string targetScene;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (DataController.instance.CheckProcress(i))
            {
                continue;
            }
            else
            {
                chapters.transform.GetChild(i + 1).GetComponent<Button>().interactable = false;
            }
        }

        settingBGSound.GetComponent<Slider>().onValueChanged.AddListener(delegate { OnBGSoundChanged(); });
        settingFXSound.GetComponent<Slider>().onValueChanged.AddListener(delegate { OnFXSoundChanged(); });
    }

    public void OpenInfo(string sceneName)
    {
        chapterInfoSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Chapter/" + sceneName);
        continueGame.GetComponent<Button>().interactable = DataController.instance.IsChapterPlayedBefore(sceneName);

        chapterInfo.SetActive(true);
        targetScene = sceneName;
    }

    public void CloseInfo()
    {
        targetScene = null;
        chapterInfo.SetActive(false);
    }

    public void OpenSetting()
    {
        if (DataController.instance.joystickFixed)
            settingFixedJoystick.GetComponent<Toggle>().isOn = true;
        else
            settingFixedJoystick.GetComponent<Toggle>().isOn = false;

        settingBGSound.GetComponent<Slider>().value = DataController.instance.bgSoundValue;
        settingFXSound.GetComponent<Slider>().value = DataController.instance.fxSoundValue;

        setting.SetActive(true);
    }

    public void CloseSetting()
    {
        setting.SetActive(false);
    }

    public void StartNewGame()
    {
        DataController.instance.ClearData(targetScene);
        AsyncLoading.LoadScene(targetScene);
    }

    public void ContinueGame()
    {
        AsyncLoading.LoadScene(targetScene);
    }

    public void ChangeJoystick()
    {
        if (!settingFixedJoystick.activeInHierarchy) return;

        if (DataController.instance.joystickFixed)
        {
            DataController.instance.joystickFixed = false;
            return;
        }
        DataController.instance.joystickFixed = true;
    }

    public void OnBGSoundChanged()
    {
        DataController.instance.bgSoundValue = settingBGSound.GetComponent<Slider>().value;
    }

    public void OnFXSoundChanged()
    {
        DataController.instance.fxSoundValue = settingFXSound.GetComponent<Slider>().value;
    }
 }
