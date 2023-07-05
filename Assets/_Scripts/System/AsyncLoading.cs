using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AsyncLoading : MonoBehaviour
{
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

    //비동기 로딩씬
    public static string nextScene;

    [SerializeField] private LoadingTexts loadingTexts;

    void Start()
    {
        Time.timeScale = 1f;
        loadingTexts = JsonUtility.FromJson<LoadingTexts>(Resources.Load<TextAsset>("Json/Script").text);
        GameObject.Find("Comment").GetComponent<TextMeshProUGUI>().text = loadingTexts.textData[Random.Range(0, loadingTexts.textData.Count + 1)].text;

        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);

                op.allowSceneActivation = true;

                yield break;
            }
        }
    }
}