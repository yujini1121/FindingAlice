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
    protected GameObject player;
    protected IEnumerator clockState;

    protected Vector3 vecToClock;

    private void Awake()
    {
        value = JsonUtility.FromJson<Value>(Resources.Load<TextAsset>("Json/Clock").text);
        player = GameObject.FindGameObjectWithTag("Player");
        clockState = ClockShoot();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        clockState = ClockShoot();
        StartCoroutine(clockState);
    }

    void ClockReturnIdle()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        StopCoroutine(clockState);
        clockState = null;

        value.clockCurDistance = 0;

        this.transform.parent = player.transform;
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    public void StateChangeToFollow()
    {
        StopCoroutine(clockState);
        clockState = ClockFollow();
        StartCoroutine(clockState);
    }

    IEnumerator ClockShoot()
    {
        value.clockStartTime = Time.time;
        Time.timeScale = value.timeScaleValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        player.GetComponent<Movement>().SendMessage("Shooting");
        Transform playerTrans = player.transform;

        while (Time.unscaledTime - value.clockStartTime < value.clockIncreasableTime + value.clockMaxDistanceTime)
        {
            if (value.clockCurDistance < value.clockMaxDistance)
            {
                value.clockCurDistance += value.clockMaxDistance * (Time.unscaledDeltaTime / value.clockIncreasableTime);
                Debug.Log(Time.unscaledTime - value.clockStartTime);
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

            yield return null;
        }

        ClockReturnIdle();
    }

    IEnumerator ClockFollow()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        transform.SetParent(null);
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = vecToClock * value.clockShootPower;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Movement>().SendMessage("Following", vecToClock * value.clockCurDistance);

            ClockReturnIdle();
        }
        else if (other.gameObject.tag == "Platform")
        {
            ClockReturnIdle();
        }
    }
}
