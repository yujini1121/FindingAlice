using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    GameObject chapterInfo;
    GameObject option;
    string targetScene;

    void Start()
    {
        chapterInfo = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        option = GameObject.Find("Canvas").transform.GetChild(3).gameObject;
    }

    public void OpenInfo(string sceneName)
    {
        chapterInfo.SetActive(true);
        chapterInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Chapter/" + sceneName);

        targetScene = sceneName;
    }

    public void CloseInfo()
    {
        targetScene = null;
        chapterInfo.SetActive(false);
    }

    public void OpenOption()
    {
        option.SetActive(true);
    }

    public void CloseOption()
    {
        option.SetActive(false);
    }

    public void ChangeScene()
    {
        AsyncLoading.LoadScene(targetScene);
    }
}
