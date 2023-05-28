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
            value.clockCurDistance += Time.unscaledDeltaTime * (value.clockMaxDistance / value.clockIncreasableTime);

            dir = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                              Input.mousePosition.y,
                                                              -Camera.main.transform.position.z))
                        - player.transform.position).normalized;

            transform.localPosition = new Vector3(dir.x * value.clockCurDistance, dir.y * value.clockCurDistance, 0);
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
            player.GetComponent<Movement>().SendMessage("Following");
            //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<Rigidbody>().AddForce(dir * value.clockCurDistance, ForceMode.Impulse);
        }
        else if (other.gameObject.tag == "Platform")
        {
            ClockReturnIdle();
        }
    }
}
