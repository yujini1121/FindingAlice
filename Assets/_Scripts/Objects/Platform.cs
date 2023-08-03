using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===================================================================================================
// Platform에 Attach되는 스크립트
//
// Inspector 창에서 어떤 플랫폼인지 ItemType을 지정하여 사용한다.
// ===================================================================================================

public class Platform : MonoBehaviour
{
    /*
    Passing Platform    : 통과할 수 있는 플랫폼
    Disappear Platform  : 밟으면 일정 시간 뒤에 사라지는 플랫폼
    Sink Platform       : 밟으면 내려가지는 플랫폼

    Button Trigger      : 버튼 누르면 플랫폼 생성

    Rip Current         : 일정 벡터로 밀리는 ch.2의 이안류 기믹
    */



    [System.Serializable]
    private enum PlatformType
    {
        Disappear,      // 밟으면 일정 시간 이후 사라지는 플랫폼
        Passing,        // 아래에서 위로 통과할 수 있는 플랫폼
        Sink,           // 밟으면 아래로 내려가는 플랫폼
        DeadZone,       // 닿으면 죽는 플랫폼
        RipCurrent      // ch.2 이안류 플랫폼
    }

    [SerializeField] private PlatformType platformType;
    private Mesh platformMesh;

    [Header("Disappear")]
    [SerializeField] private float takesToDisappear;
    [SerializeField] private float takesToAppear;
    private Coroutine disappearTimeCheck;

    [Header("Sink")]
    [SerializeField] private float sinkMaxDistance;
    [SerializeField] private float sinkSpeed;
    private Coroutine sink;
    private Coroutine uprise;
    private Vector3 originPos;

    [Header("RipCurrent")]
    [SerializeField] private Vector3 vectorOfRipCurrent;

    private Ch2_Movement ch2_Movement;

    private void Start()
    {
        platformMesh = GetComponent<MeshFilter>().mesh;
        originPos = transform.position;
        ch2_Movement = GameObject.Find("Player").GetComponent<Ch2_Movement>();

        switch (platformType)
        {
            case PlatformType.Disappear:
                GetComponent<Collider>().isTrigger = false;
                break;

            case PlatformType.Passing:
                GetComponent<Collider>().isTrigger = true;
                break;

            case PlatformType.Sink:
                GetComponent<Collider>().isTrigger = false;
                break;

            case PlatformType.DeadZone:
                GetComponent<Collider>().isTrigger = true;
                break;

            case PlatformType.RipCurrent:
                GetComponent<Collider>().isTrigger = true;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Disappear:
                    if (collision.transform.position.y - transform.position.y > transform.localScale.y / 2)
                    {
                        if (disappearTimeCheck == null)
                            disappearTimeCheck = StartCoroutine(DisappearTimeCheck(takesToDisappear));
                    }
                    break;

                case PlatformType.Passing:
                    break;

                case PlatformType.Sink:
                    if (collision.transform.position.y - transform.position.y > transform.localScale.y / 2)
                    {
                        collision.transform.parent = transform;

                        if (uprise != null)
                        {
                            StopCoroutine(uprise);
                            uprise = null;
                        }
                        sink = StartCoroutine(Sink());
                    }
                    break;

                case PlatformType.DeadZone:
                    break;

                case PlatformType.RipCurrent:
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Disappear:
                    break;

                case PlatformType.Passing:
                        GetComponent<Collider>().isTrigger = true;
                    break;

                case PlatformType.Sink:
                    if (collision.transform.parent == gameObject.transform)
                    {
                        collision.transform.parent = null;

                        if (sink != null)
                        {
                            StopCoroutine(sink);
                            sink = null;
                        }
                        uprise = StartCoroutine(Uprise());
                    }
                    break;

                case PlatformType.DeadZone:
                    break;

                case PlatformType.RipCurrent:
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Disappear:
                    break;

                case PlatformType.Passing:
                    if (Vector3.Dot(Vector3.up, other.GetComponent<Rigidbody>().velocity) < 0)
                    {
                        GetComponent<Collider>().isTrigger = false;
                    }
                    break;

                case PlatformType.Sink:
                    break;

                case PlatformType.DeadZone:
                    GameManager.instance.PlayerDead();
                    break;

                case PlatformType.RipCurrent:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Disappear:
                    break;

                case PlatformType.Passing:
                    break;

                case PlatformType.Sink:
                    break;

                case PlatformType.DeadZone:
                    break;

                case PlatformType.RipCurrent:
                    ch2_Movement.EnterRipCurrent(vectorOfRipCurrent);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Disappear:
                    break;

                case PlatformType.Passing:
                    break;

                case PlatformType.Sink:
                    break;

                case PlatformType.DeadZone:
                    break;

                case PlatformType.RipCurrent:
                    ch2_Movement.ExitRipCurrent();
                    break;
            }
        }
    }

    private IEnumerator DisappearTimeCheck(float t)
    {
        yield return new WaitForSeconds(t);
        GetComponent<MeshFilter>().mesh = null;
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(takesToAppear);
        GetComponent<MeshFilter>().mesh = platformMesh;
        GetComponent<BoxCollider>().enabled = true;

        disappearTimeCheck = null;
    }
    

    private IEnumerator Sink()
    {
        while (originPos.y - transform.position.y < sinkMaxDistance)
        {
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Uprise()
    {
        while (originPos.y > transform.position.y)
        {
            transform.Translate(Vector3.up * sinkSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
