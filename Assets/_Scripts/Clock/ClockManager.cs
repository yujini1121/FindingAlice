using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    //public static ClockManager instance;

    //private void Awake()
    //{
    //    if (instance == null) instance = this;
    //    else if (instance != this) Destroy(gameObject);
    //}


    public GameObject clock;

    void Start()
    {
        clock = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                clock.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                clock.GetComponent<Clock>().SendMessage("StateChangeToFollow");
            }
        }
    }
}
