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

    [System.Serializable]
    private class Value
    {
        public float timeScaleValue;

        public float clockIncreasableTime;
        public float clockMaxDistanceTime;
        public float clockStartTime;

        public float clockMaxDistance;
        public float clockCurDistance;
        public float clockShootPower;
    }

    Value value;
    GameObject player;
    GameObject clock;
    IEnumerator clockState;

    Vector3 dir;

    void Start()
    {
        value = JsonUtility.FromJson<Value>(Resources.Load<TextAsset>("Json/Clock").text);

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
            else if (Input.GetMouseButtonUp(0))
            {
                StopCoroutine(clockState);

                clockState = ClockFollow();
                StartCoroutine(clockState);
            }
        }
    }

    IEnumerator ClockShoot()
    {
        value.clockStartTime = Time.time;
        Time.timeScale = value.timeScaleValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        player.GetComponent<Movement>().SendMessage("Shooting");

        while (Time.unscaledTime - value.clockStartTime < value.clockIncreasableTime + value.clockMaxDistanceTime)
        {
            value.clockCurDistance += Time.unscaledDeltaTime * (value.clockMaxDistance / value.clockIncreasableTime);

            dir = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                              Input.mousePosition.y,
                                                              -Camera.main.transform.position.z))
                        - player.transform.position).normalized;

            clock.transform.localPosition = new Vector3(dir.x * value.clockCurDistance, dir.y * value.clockCurDistance, 0);
            clock.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg));
            yield return null;
        }
    }

    IEnumerator ClockFollow()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        clock.transform.SetParent(null);
        player.GetComponent<Rigidbody>().useGravity = false;
        //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = dir * value.clockShootPower;

        yield return null;
    }
}
