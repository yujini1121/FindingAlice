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
}
