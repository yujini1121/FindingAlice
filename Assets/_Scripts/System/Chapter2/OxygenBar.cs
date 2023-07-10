using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    private Slider oxygenBar;

    private float maxOxygen;
    private float minOxygen;
    private float depletionRate;

    private void Start()
    {
        maxOxygen = 1f;
        minOxygen = 0f;

        depletionRate = maxOxygen / 15f;

        oxygenBar = gameObject.GetComponent<Slider>();
        oxygenBar.value = maxOxygen;
    }

    private void LateUpdate() 
    {
        if (oxygenBar.value <= maxOxygen && oxygenBar.value > minOxygen)
        {
            oxygenBar.value -= depletionRate * Time.deltaTime;
        }
        else
        {
            oxygenBar.value = 0f;
            GameManager.instance.PlayerDead();
        }
    }
}
