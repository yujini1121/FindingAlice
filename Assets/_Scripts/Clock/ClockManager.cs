using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// ===================================================================================================
// ClockManager 오브젝트에 Attach되는 스크립트

// 시계와 플레이어를 중재하고, 시계와 관련된 UI처리를 한다.
// ===================================================================================================

public class ClockManager : MonoBehaviour
{
    public static ClockManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private GameObject clock;
    private List<GameObject> clockUI;
    private Coroutine clockReload;

    private int clockMaxCount = 2;
    [SerializeField] private int _clockCounter;
    [SerializeField] private float clockReloadTime = 3f;
    [SerializeField] private bool _clockShootable = true;

    // ===============================================================================================
    // 시계 발사 횟수를 관리하는 프로퍼티
    //
    // 증감 연산자에 따라 시계를 활성/비활성화
    // ===============================================================================================
    public int clockCounter
    {
        get
        {
            return _clockCounter;
        }
        set
        {
            int temp = _clockCounter;
            _clockCounter = Mathf.Clamp(value, 0, clockMaxCount);

            // clockCounter++
            if (_clockCounter > temp)
            {
                //for (int i = 0; i < _clockCounter; i++)
                //{
                //    clockUI[i].SetActive(true);
                //}
                clockUI[_clockCounter - 1].SetActive(true);
            }
            // clockCounter--
            else if (_clockCounter < temp)
            {
                clockUI[_clockCounter].GetComponent<Image>().fillAmount = 0;

                if (clockReload != null)
                {
                    clockUI[_clockCounter].GetComponent<Image>().fillAmount = clockUI[temp].GetComponent<Image>().fillAmount;
                    clockUI[temp].GetComponent<Image>().fillAmount = 0;
                    StopCoroutine(clockReload);
                }

            }
        }
    }

    // ===============================================================================================
    // 시계를 사용할 수 없는 상태(Dialogue 출력 등)를 관리하는 프로퍼티
    // ===============================================================================================
    public bool clockShootable
    {
        get
        {
            return _clockShootable;
        }
        set
        {
            _clockShootable = value;
        }
    }

    private void Start()
    {
        clock = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        clockUI = new List<GameObject>();
        clockCounter = clockMaxCount;

        for (int i = 0; i < clockMaxCount; i++)
        {
            clockUI.Add(GameObject.Find("ClockCounter").transform.GetChild(i).gameObject);
        }
    }

    // ===============================================================================================
    // 시계 사용 입력이 주어졌을 때 호출되는 함수
    // ===============================================================================================
    public void ClockBegin()
    {
        if (clockCounter > 0 && clockShootable)
        {
            clock.SetActive(true);
        }
    }

    // ===============================================================================================
    // 시계 사용 입력이 종료되었을 때 호출되는 함수
    // ===============================================================================================
    public void ClockEnd()
    {
        if (clock.activeInHierarchy)
        {
            clock.GetComponent<Clock>().ClockFollow();
            clockCounter--;

            if (SceneManager.GetActiveScene().name != "Chapter_2")
            {
                ClockResume();
            }
        }
    }

    // ===============================================================================================
    // 일정 시간 시간 후에 시계를 발사할 수 있는 횟수를 증가시키는 함수
    //
    // 실행 후 시계가 최대가 아닐 경우 재실행
    // ===============================================================================================
    private IEnumerator ClockReload()
    {
        while (true)
        {
            if (clockCounter >= 0 && clockCounter < clockUI.Count)
            {
                clockUI[clockCounter].GetComponent<Image>().fillAmount += Time.deltaTime * (1 / clockReloadTime);
                if(clockUI[clockCounter].GetComponent<Image>().fillAmount >= 1)
                {
                    break;
                }
            }
            yield return null;

        }
        clockUI[clockCounter].SetActive(false);


        clockCounter++;

        if (clockCounter < clockMaxCount)
        {
            clockReload = StartCoroutine(ClockReload());
        }
        else
        {
            clockReload = null;
        }
    }

    public void ClockCoroutinePause()
    {
        if (clockReload != null)
        {
            StopCoroutine(clockReload);
        }
    }

    public void ClockCoroutineStart()
    {
        ClockResume();  
    }

    private void ClockResume()
    {
        clockReload = StartCoroutine(ClockReload());
    }

    // ===============================================================================================
    // 시계의 값을 원복시키는 함수
    // ===============================================================================================
    public void ClockReturnIdle()
    {
        if (clock.activeSelf)
        {
            clock.GetComponent<Clock>().ClockReturnIdle();
        }
    }
}