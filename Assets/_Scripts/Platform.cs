using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Platform : MonoBehaviour
{
    /*
    Passing Platform    : 통과할 수 있는 플랫폼
    Disappear Platform  : 밟으면 일정 시간 뒤에 사라지는 플랫폼
    Sink Platform       : 밟으면 내려가지는 플랫폼

    Button Trigger      : 버튼 누르면 플랫폼 생성
    */

    [System.Serializable]
    public enum PlatformType
    {
        Disappear,
        Passing,
        Sink
    }

    Mesh platformMesh;

    [SerializeField] private PlatformType platformType;

    [Header("Disappear")]
    [SerializeField] private float takesToDisappear;
    [SerializeField] private float takesToAppear;
    Coroutine disappearTimeCheck;

    [Header("Sink")]
    [SerializeField] private float sinkMaxDistance;
    [SerializeField] private float sinkSpeed;
    Coroutine sink;
    Coroutine uprise;
    Vector3 originPos;

    void Start()
    {
        platformMesh = GetComponent<MeshFilter>().mesh;
        originPos = transform.position;

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
