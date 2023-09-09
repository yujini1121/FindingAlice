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
    private int playerDir;  // 해파리와 충돌했을 때, 플레이어가 밀려나는 방향

    private Vector3 originalPosition;
    private GameObject playerObject;
    private Transform playerTransform;
    private Rigidbody playerRb;


    private void Start()
    {
        originalPosition = transform.position;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
        playerRb = playerObject.GetComponent<Rigidbody>();

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
                    playerDir = playerObject.transform.position.x - gameObject.transform.position.x > 0 ? 1 : -1;
                    playerRb.AddForce(new Vector3(playerDir * 150, 0, 0), ForceMode.Impulse);
                    // playerTransform.position = Vector3.Lerp(playerTransform.position, new Vector3(playerDir * 150, 0, 0), 0.2f);
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

    private IEnumerator ReturnOriginalPos()
    {
        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator JelMove()
    {
        while (true)
        {
            transform.position = originalPosition + new Vector3(0, Mathf.Sin(Time.time) * 2.5f, 0);
            yield return null;
        }
    }
}