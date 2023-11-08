using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public static OxygenBar instance;

    private Image oxygenBar;

    private float curTime;
    private float maxOxygen;
    public static float curOxygen;

    private float oxygenItem;
    private float caveOxygenRate;
    public float OxygenRatio;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        oxygenBar = GetComponent<Image>();

        curTime = 0f;
        maxOxygen = 100f;
        curOxygen = maxOxygen;

        // depletionRate = maxOxygen / 1000f; 
        // oxygenItem = maxOxygen / 2f;
        // caveOxygenRate = maxOxygen / 5f;

        // StartCoroutine(OxygenBarUpdate());
    }

    private void Update()
    {
        oxygenBar.fillAmount = curOxygen / maxOxygen;
        curTime += Time.deltaTime;

        if (curTime >= 3.0f)
        {
            curOxygen -= 12.5f;
            curOxygen = Mathf.Max(0, curOxygen);
            curTime = 0f;
        }

        if (curOxygen <= 0)
        {
            oxygenBar.fillAmount = 0f;
            GameManager.instance.PlayerDead();
        }
    } 

    // // 임시로 while true로 작성, 추후 수정 필요
    // private IEnumerator OxygenBarUpdate()
    // {
    //     while (true)
    //     {
    //         oxygenBar.fillAmount = curOxygen / maxOxygen;

    //         curTime += Time.deltaTime;
    //         Debug.Log(curTime);

    //         if (curTime >= 3.0f)
    //         {
    //             curOxygen -= 12.5f;
    //             curOxygen = Mathf.Max(0, curOxygen);
    //             curTime = 0f;

    //             yield return null;
    //         }
    //     }
    // }

    // 예전코드 ######################################################################
    // private IEnumerator OxygenBarUpdate()
    // {
        
    //     while (oxygenBar.value <= maxOxygen && oxygenBar.value > minOxygen)
    //     {
    //         OxygenRatio = oxygenBar.value;
    //         oxygenBar.value -= depletionRate * Time.deltaTime;

    //         yield return null;
    //     }

    //     // oxygenBar의 값이 0이면, 플레이어 사망
    //     oxygenBar.value = 0f;
    //     GameManager.instance.PlayerDead();
    // }

    public void GetOxygenItem()
    {
        Debug.Log("GetOxygenItem");
        oxygenBar.fillAmount += oxygenItem;
    }

    public void EnterCave()
    {
        Debug.Log("EnterCave");
        oxygenBar.fillAmount += caveOxygenRate * Time.deltaTime;
    }
}