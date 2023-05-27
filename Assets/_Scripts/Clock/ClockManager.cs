using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public static ClockManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    GameObject player;
    GameObject clock;
    IEnumerator clockState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        clock = player.transform.Find("Clock").gameObject;
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
                clockState = ClockShoot();
                StartCoroutine(clockState);
            }
        }
    }

    IEnumerator ClockShoot()
    {
        player.GetComponent<Movement>().
    }
}
