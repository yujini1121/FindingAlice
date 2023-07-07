using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DataController : MonoBehaviour
{
    public static DataController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        gameData = JsonUtility.FromJson<GameData>(Resources.Load<TextAsset>("Json/GameData").text);

        gameData.Print();
    }

    [System.Serializable]
    public class GameData
    {
        public int totalProgressFlag;

        public int chapterFlag_CT;
        public int chapterFlag_C1;
        public int chapterFlag_C2;
        public int chapterFlag_C3;

        public int collectionFlag_CT;
        public int collectionFlag_C1;
        public int collectionFlag_C2;
        public int collectionFlag_C3;

        public void Print()
        {
            Debug.Log(totalProgressFlag);
            Debug.Log(chapterFlag_CT);
            Debug.Log(chapterFlag_C1);
            Debug.Log(chapterFlag_C2);
            Debug.Log(chapterFlag_C3);
            Debug.Log(collectionFlag_CT);
            Debug.Log(collectionFlag_C1);
            Debug.Log(collectionFlag_C2);
            Debug.Log(collectionFlag_C3);
        }
    }

    [SerializeField] GameData gameData;

    void Start()
    {
        LoadData();

        //gameData.Print();
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
        }

        string toJsonData   = JsonUtility.ToJson(gameData, true);
        string filePath     = Application.dataPath + "/Resources/Json/GameData.json";
        File.WriteAllText(filePath, toJsonData);
    }

    public void LoadData()
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
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C1, 3).Length - 1).ToString()).transform.position;
                }
                break;
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = position;
    }

    public void GetCollection()
    {

    }
}
