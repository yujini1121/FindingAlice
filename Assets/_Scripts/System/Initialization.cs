using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// ===================================================================================================
// 게임이 실행되었을 때 Title 씬에서 동작하는 스크립트
// ===================================================================================================

public class Initialization : MonoBehaviour
{

    [SerializeField] private GameObject copyrightBtn;
    [SerializeField] private GameObject copyrightInfo;
    [SerializeField] private GameObject touchToStart;
    private bool gameStartReady;
    float time;
    

    //private void Awake()
    //{
    //    Camera camera = GetComponent<Camera>();
    //    Rect rect = camera.rect;
    //    float scaleheight = ((float)Screen.width / Screen.height) / ((float)20 / 9); // (가로 / 세로)
    //    float scalewidth = 1f / scaleheight;
    //    if (scaleheight < 1)
    //    {
    //        rect.height = scaleheight;
    //        rect.y = (1f - scaleheight) / 2f;
    //    }
    //    else
    //    {
    //        rect.width = scalewidth;
    //        rect.x = (1f - scalewidth) / 2f;
    //    }
    //    camera.rect = rect;
    //}

    //void OnPreCull() => GL.Clear(true, true, Color.black);

    private void Start()
    {
        gameStartReady = true;
        AudioManager.instance.PlayBgm(true);
    }

    private void Update()
    {
        BlinkAnimation();
    }

    public void StartGame()
    {
        if (gameStartReady)
        {
            AsyncLoading.LoadScene("ChapterSelect");
        }
    }

    public void OpenCopyRightInfo()
    {
        copyrightBtn.SetActive(false);
        copyrightInfo.SetActive(true);
    }

    public void CloseCopyRightInfo()
    {
        copyrightBtn.SetActive(true);
        copyrightInfo.SetActive(false);
    }

    private void BlinkAnimation()
    {
        if (time < 0.5f) touchToStart.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 1 - time);
        else
        {
            touchToStart.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, time);
            if (time > 1f) time = 0;
        }

        time += Time.deltaTime;
    }
}
