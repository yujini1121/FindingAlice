using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnmuteManager : MonoBehaviour
{
    [SerializeField] private GameObject bgUnmute;
    [SerializeField] private GameObject fxUnmute;

    private void Update()
    {
        bgUnmute.SetActive(AudioManager.instance.bgmVolume > 0);
        fxUnmute.SetActive(AudioManager.instance.sfxVolume > 0);
    }
}
