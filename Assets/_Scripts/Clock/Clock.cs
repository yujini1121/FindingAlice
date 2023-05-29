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

    protected Vector3 dir;

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
        StartCoroutine(clockState);
    }

    void ClockReturnIdle()
    {

    }

    void StateChangeToFollow()
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

        while (Time.unscaledTime - value.clockStartTime < value.clockIncreasableTime + value.clockMaxDistanceTime)
        {
            if (value.clockCurDistance < value.clockMaxDistance)
                value.clockCurDistance += value.clockMaxDistance * (Time.unscaledDeltaTime / value.clockIncreasableTime);

            dir = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                              Input.mousePosition.y,
                                                              -Camera.main.transform.position.z))
                        - player.transform.position).normalized;

            if ((dir.x < 0 && player.transform.localScale.x > 0) || (dir.x > 0 && player.transform.localScale.x < 0))
            {
                Vector3 sightDir = player.transform.localScale;
                sightDir.x *= -1;
                player.transform.localScale = sightDir;
            }

            transform.localPosition = new Vector3(dir.x * value.clockCurDistance * (player.transform.localScale.x / Mathf.Abs(player.transform.localScale.x)),
                                                    dir.y * value.clockCurDistance, 0);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg));

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
        player.GetComponent<Rigidbody>().velocity = dir * value.clockShootPower;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Movement>().SendMessage("Following", dir * value.clockCurDistance);
        }
        else if (other.gameObject.tag == "Platform")
        {
            ClockReturnIdle();
        }
    }
}
