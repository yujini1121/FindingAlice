using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public static OxygenBar instance;

    private Image oxygenBar;

    private float maxOxygen;
    private float curOxygen;
    private float curTime;

    private float depletionRate;
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

        // oxygenBar.value = maxOxygen;

        StartCoroutine(OxygenBarUpdate());
    }

    private IEnumerator OxygenBarUpdate()
    {
        while (true)
        {
            oxygenBar.fillAmount = curOxygen / maxOxygen;

            curTime += Time.deltaTime;
            Debug.Log(curTime);

            if (curTime >= 3.0f)
            {
                curOxygen -= 12.5f;
                curOxygen = Mathf.Max(0, curOxygen);
                curTime = 0f;

                yield return null;
            }  
        }      



        // while (oxygenBar.fillAmount <= maxOxygen && oxygenBar.fillAmount > 0f)
        // {
        //     OxygenRatio = oxygenBar.fillAmount;
        //     oxygenBar.fillAmount -= depletionRate * Time.deltaTime;

        //     yield return null;
        // }

        // // oxygenBar의 값이 0이면, 플레이어 사망
        // oxygenBar.fillAmount = 0f;
        // GameManager.instance.PlayerDead();
    }

    public void GetOxygenItem()
    {
        oxygenBar.fillAmount += oxygenItem;
    }

    public void EnterCave()
    {
        oxygenBar.fillAmount += caveOxygenRate * Time.deltaTime;
    }
}