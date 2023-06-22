using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [System.Serializable]
    protected class Value
    {
        public float timeScaleValue;

        public float clockIncreasableTime;
        public float clockMaxDistanceTime;
        public float clockStartTime;

        public float clockMaxDistance;
        public float clockCurDistance;
        public float clockShootPower;
    }

    protected Value value;
    private GameObject player;
    Transform playerTrans;

    private Vector3 vecToClock;

    bool isClockOnEnable;

    private void Awake()
    {
        value = JsonUtility.FromJson<Value>(Resources.Load<TextAsset>("Json/Clock").text);
        player = GameObject.FindGameObjectWithTag("Player");
        //ClockReturnIdle();
    }

    void Start()
    {
    }

    void Update()
    {
        if (isClockOnEnable && Time.unscaledTime - value.clockStartTime < value.clockIncreasableTime + value.clockMaxDistanceTime)
        {
            if (value.clockCurDistance < value.clockMaxDistance)
            {
                value.clockCurDistance += (value.clockMaxDistance / value.clockIncreasableTime) * Time.unscaledDeltaTime;
            }

            vecToClock = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                              Input.mousePosition.y,
                                                              -Camera.main.transform.position.z))
                        - player.transform.position).normalized;

            if ((vecToClock.x < 0 && playerTrans.localScale.x > 0) || (vecToClock.x > 0 && playerTrans.localScale.x < 0))
            {
                Vector3 sightDir = playerTrans.localScale;
                sightDir.x *= -1;
                playerTrans.localScale = sightDir;
            }

            transform.localPosition = new Vector3(vecToClock.x * value.clockCurDistance * (playerTrans.localScale.x / Mathf.Abs(playerTrans.localScale.x)),
                                                    vecToClock.y * value.clockCurDistance, 0);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(vecToClock.x, vecToClock.y) * Mathf.Rad2Deg));
        }
    }

    private void OnEnable()
    {
        isClockOnEnable = true;
        playerTrans = player.transform;

        value.clockStartTime = Time.unscaledTime;
        Time.timeScale = value.timeScaleValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        player.GetComponent<Movement>().StateBeginShoot();
    }

    public void ClockReturnIdle()
    {
        isClockOnEnable = false;

        if (Time.timeScale != 1)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        value.clockCurDistance = 0;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void ClockFollow()
    {
        isClockOnEnable = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        transform.SetParent(null);
        player.GetComponent<Movement>().StateBeginFollow(vecToClock * value.clockShootPower);
        //player.GetComponent<Rigidbody>().useGravity = false;
        //player.GetComponent<Rigidbody>().velocity = vecToClock * value.clockShootPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Movement>().StateCollsionWithClock(vecToClock * value.clockCurDistance);
            ClockReturnIdle();
        }
        else if (other.gameObject.tag == "Platform")
        {
            ClockReturnIdle();
        }
    }
}