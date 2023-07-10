using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    public static DataController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        gameData     = JsonUtility.FromJson<GameData>(Resources.Load<TextAsset>("Json/GameData").text);
        loadingTexts = JsonUtility.FromJson<LoadingTexts>(Resources.Load<TextAsset>("Json/Script").text);
    }

    [System.Serializable]
    public class TextData
    {
        public string text;
    }

    [System.Serializable]
    public class LoadingTexts
    {
        public List<TextData> textData;
    }

    [System.Serializable]
    public class GameData
    {
        public int totalProgressFlag;

        public int  chapterFlag_CT;
        public int  chapterFlag_C1;
        public int  chapterFlag_C2;
        public int  chapterFlag_C3;

        public int  collectionFlag_CT;
        public int  collectionFlag_C1;
        public int  collectionFlag_C2;
        public int  collectionFlag_C3;

        public bool joystickFixed;
    }

    [SerializeField] private GameData gameData;
    [SerializeField] public LoadingTexts loadingTexts;

    public bool joystickFixed
    {
        get { return gameData.joystickFixed; }
        set { gameData.joystickFixed = value; }
    }

    // 플레이 기록이 있는 챕터면 true 반환
    public bool IsChapterPlayedBefore(string sceneName)
    {
        switch (sceneName)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT == 0)
                    return false;
                return true;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 == 0)
                    return false;
                return true;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 == 0)
                    return false;
                return true;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 == 0)
                    return false;
                return true;
        }

        return false;
    }

    public void ClearData(string sceneName)
    {
        switch (sceneName)
        {
            case "Chapter_T":
                gameData.chapterFlag_CT &= 0;
                break;

            case "Chapter_1":
                gameData.chapterFlag_C1 &= 0;
                break;

            case "Chapter_2":
                gameData.chapterFlag_C2 &= 0;
                break;

            case "Chapter_3":
                gameData.chapterFlag_C3 &= 0;
                break;
        }

        SaveData(0);
    }

    public void SaveData(int bitPlace)
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                // 현재 저장 위치가 이미 저장되어 있다면
                if ((gameData.chapterFlag_CT & bitPlace) == bitPlace)
                {
                    return;
                }
                gameData.chapterFlag_CT |= bitPlace;
                break;

            case "Chapter_1":
                if ((gameData.chapterFlag_C1 & bitPlace) == bitPlace)
                {
                    return;
                }
                gameData.chapterFlag_C1 |= bitPlace;
                break;

            case "Chapter_2":
                if ((gameData.chapterFlag_C2 & bitPlace) == bitPlace)
                {
                    return;
                }
                gameData.chapterFlag_C2 |= bitPlace;
                break;

            case "Chapter_3":
                if ((gameData.chapterFlag_C3 & bitPlace) == bitPlace)
                {
                    return;
                }
                gameData.chapterFlag_C3 |= bitPlace;
                break;

            default:
                break;
        }

        string toJsonData   = JsonUtility.ToJson(gameData, true);
        string filePath     = Application.dataPath + "/Resources/Json/GameData.json";
        File.WriteAllText(filePath, toJsonData);
        Debug.Log("Save Complete");
    }

    public void LoadSavePoint()
    {
        Vector3 position = GameObject.FindGameObjectWithTag("Player").transform.position;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_CT, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C1, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C2, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C3, 2).Length - 1).ToString()).transform.position;
                }
                break;
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = position;
    }

    // true 반환하면 해당 챕터 잠금 해제
    public bool CheckProcress(int i)
    {
        if (gameData.totalProgressFlag == 0)
        {
            return false;
        }
        else if (i <= Convert.ToString(gameData.totalProgressFlag, 2).Length - 1)
        {
            return true;
        }

        return false;
    }

    public bool CheckArea(int area)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_CT, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C1, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C2, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C3, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    public void GetCollection()
    {

    }
}
