using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    GameObject chapters;
    GameObject chapterInfo;
    GameObject setting;
    string targetScene;

    void Start()
    {
        chapters = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        chapterInfo = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        setting = GameObject.Find("Canvas").transform.GetChild(3).gameObject;

        for (int i = 0; i < 3; i++)
        {
            if (DataController.instance.CheckProcress(i))
            {
                continue;
            }
            else
            {
                chapters.transform.GetChild(i + 4).gameObject.SetActive(true);
            }
        }
    }

    public void OpenInfo(string sceneName)
    {
        chapterInfo.SetActive(true);
        chapterInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Chapter/" + sceneName);

        chapterInfo.transform.GetChild(0).GetChild(3).gameObject.SetActive(DataController.instance.IsChapterPlayedBefore(sceneName));

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
