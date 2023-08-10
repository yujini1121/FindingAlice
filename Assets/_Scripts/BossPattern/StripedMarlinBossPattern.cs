using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StripedMarlinBossPattern : MonoBehaviour
{
    private Coroutine pattern;

    private GameObject player;

    [SerializeField] private GameObject spearfishPrefab;
    private Transform warningSpearfish;
    private Rigidbody spearfishRb;
    private Vector3 randomPosition;

    private MeshRenderer testWarning;
    private MeshRenderer warningSpearfishMesh;

    private float patternCooldown = 4f;
    private float patternWarningTime = 3f;
    private float spearfishWarningTime = 0.5f;
    private float spearfishSpeed = 8f;

    private void Awake()
    {
        warningSpearfish = transform.GetChild(1);
        testWarning = transform.GetChild(0).GetComponent<MeshRenderer>();
        warningSpearfishMesh = warningSpearfish.GetComponent<MeshRenderer>();
        spearfishRb = spearfishPrefab.GetComponent<Rigidbody>();    

    }

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        StartCoroutine(FollowPlayer());

        if (pattern != null) pattern = null;
        pattern = StartCoroutine(Pattern());
    }

    // 플레이어의 위치를 반영해 따라 다님
    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = new Vector3(player.transform.position.x,
                                             player.transform.position.y,
                                             gameObject.transform.position.z);
            yield return null;                                        
        }
    }

    private IEnumerator Pattern()
    {
        // * 0, 2가 맞음, 테스트를 위해 잠시 1, 2로 변경한 것 *
        int patternCase = Random.Range(0, 1);
        float patternStartTime = Time.time;

        switch (patternCase)
        {
            // 1페이즈 : 청새치 돌진
            case 0:
                spearfishRb.velocity = Vector3.zero;

                randomPosition = new Vector3(
                    Random.Range(player.transform.position.x - 11f, player.transform.position.x + 11f),
                    (Random.value > 0.5f ? player.transform.position.y + 8f : player.transform.position.y - 8f),
                    0f);

                Vector3 direction = (randomPosition - player.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                warningSpearfish.rotation = Quaternion.Euler(0f, 0f, angle);
                spearfishPrefab.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

                warningSpearfish.position = player.transform.position + new Vector3(0, 0, -1);
                float beforeX = warningSpearfish.transform.position.x;
                warningSpearfish.gameObject.SetActive(true);

                Color warningColor = warningSpearfishMesh.material.color;

                while (Time.time - patternStartTime < spearfishWarningTime)
                {
                    float alpha = (Time.time - patternStartTime) / spearfishWarningTime;
                    warningColor.a = alpha; 
                    warningSpearfishMesh.material.color = warningColor;
                    yield return null;
                }

                warningSpearfish.gameObject.SetActive(false);
                float afterX = warningSpearfish.gameObject.transform.position.x;

                spearfishPrefab.transform.position = randomPosition + new Vector3(afterX - beforeX, 0, 0);

                spearfishRb.velocity = -spearfishPrefab.transform.up * spearfishSpeed;
                warningSpearfish.gameObject.SetActive(false);

                yield return new WaitForSeconds(2.5f);
                break;

            // 2페이즈 : 전원 돌격
            case 1:
                // 패턴 예고
                transform.GetChild(0).gameObject.SetActive(true);
                Color color = testWarning.material.color;


                while (Time.time - patternStartTime < patternWarningTime)
                {
                    float alpha = (Time.time - patternStartTime) / patternWarningTime;
                    color = new Color(color.r, color.g, color.b, alpha);
                    //Debug.Log(alpha);

                    testWarning.material.color = color;
                    yield return null;
                }
                yield return new WaitForSeconds(patternCooldown);
                break;
        }
        

        pattern = StartCoroutine(Pattern());
    }

    // while (pattern2_duration <= 1f)
    // {
    //     pattern.transform.position = new Vector3(player.transform.position.x,
    //                                                 player.transform.position.y,
    //                                                 pattern.transform.position.z);
    //     pattern2_time += Time.deltaTime;
    //     pattern2_duration = pattern2_time / pattern2_launchTime;
    //     patternColor.material.color = new Color(1, 0, 0, Mathf.Lerp(0f, 0.8f, pattern2_duration));
    //     yield return null;
    // }
}
