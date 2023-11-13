using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnmuteManager : MonoBehaviour
{
    [SerializeField] private GameObject bgmute;
    [SerializeField] private GameObject fxmute;

    private void Update()
    {
        bgmute.SetActive(AudioManager.instance.bgmVolume <= 0);
        fxmute.SetActive(AudioManager.instance.sfxVolume <= 0);
    }
}
