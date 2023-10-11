using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGauge : MonoBehaviour
{
    private Image OxygenBar;
    private float maxOxygen = 100f;
    public static float curOxygen;

    private float curTime;

    void Start()
    {
        OxygenBar = GetComponent<Image>();
        curOxygen = maxOxygen;
        curTime = 0f;
    }

    void Update()
    {
        OxygenBar.fillAmount = curOxygen / maxOxygen;

        curTime += Time.deltaTime;

        if (curTime >= 3.0f)
        {
            curOxygen -= 12.5f;
            curOxygen = Mathf.Max(0, curOxygen);
            curTime = 0f;
        }
    } 
}