using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public static OxygenBar instance;

    private Image oxygenBar;

    private float maxOxygen;
    private float minOxygen;

    private float depletionRate;
    private float oxygenItem;
    private float caveOxygenRate;
    public float OxygenRatio;

    private float currentOxygen;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        maxOxygen = 1f; 
        minOxygen = 0f;

        depletionRate = maxOxygen / 15f; 
        oxygenItem = maxOxygen / 2f;
        caveOxygenRate = maxOxygen / 5f;

        currentOxygen = maxOxygen;

        oxygenBar = gameObject.GetComponent<Image>();
        oxygenBar.fillAmount = maxOxygen;

        StartCoroutine(OxygenBarUpdate());
    }

    private IEnumerator OxygenBarUpdate()
    {

        while (oxygenBar.fillAmount <= maxOxygen && oxygenBar.fillAmount > minOxygen)
        {
            OxygenRatio = oxygenBar.fillAmount;
            oxygenBar.fillAmount -= depletionRate * Time.deltaTime;

            yield return null;
        }

        // oxygenBar의 값이 0이면, 플레이어 사망
        oxygenBar.fillAmount = 0f;
        GameManager.instance.PlayerDead();
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