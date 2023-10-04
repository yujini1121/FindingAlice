using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [System.Serializable]
    private enum FishType
    {
        FollowingFish,
        JellyFish
    }

    [SerializeField] private FishType fishType;

    [Header("FollowingFish")]
    [SerializeField] private float followSpeed = 3.0f; 
    [SerializeField] private float returnSpeed = 6.0f;
    
    [Header("JellyFish")]
    private int knockBackDir;

    private Vector3 originalPosition;
    private GameObject player;
    private Transform playerTransform;
    private Rigidbody playerRb;
    private int playerDir;
    private Vector3 knockBackPos;

    private void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();

        switch (fishType)
        {
            case FishType.FollowingFish:
                GetComponent<Collider>().isTrigger = true;
                break;
            case FishType.JellyFish:
                StartCoroutine(JelMove());
                break;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    break;
                case FishType.JellyFish:
                    playerDir = player.transform.position.x - gameObject.transform.position.x > 0 ? 1 : -1;
                    playerRb.AddForce(new Vector3(playerDir * 150, 0, 0), ForceMode.Impulse);

                    // 플레이어가 밀려나야 하는 방향 (플레이어의 오른쪽에서 부딪히면 -1, 왼쪽에서 부딪히면 1)
                    Debug.Log("KnockBack");
                    knockBackDir = player.transform.position.x - gameObject.transform.position.x > 0 ? 1 : -1;
                    knockBackPos = new Vector3(playerTransform.position.x + 4f * knockBackDir, 
                                               playerTransform.position.y + 4f, 
                                               playerTransform.position.z);
                    Debug.Log(knockBackPos);
                    playerTransform.position = Vector3.Lerp(playerTransform.position, knockBackPos, 1f);

                    // playerRb.AddForce(new Vector3(knockBackDir * 2f, 0f, 0f) * 100f, ForceMode.Impulse);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
                    break;
                case FishType.JellyFish:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    StartCoroutine(ReturnOriginalPos());
                    break;
                case FishType.JellyFish:
                    break;
            }
        }
    }

    // FollowingFish
    private IEnumerator ReturnOriginalPos()
    {
        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // JellyFish
    private IEnumerator JelMove()
    {
        while (true)
        {
            transform.position = originalPosition + new Vector3(0, Mathf.Sin(Time.time) * 2.5f, 0);
            yield return null;
        }
    }
}