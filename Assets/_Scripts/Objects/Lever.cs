using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject testPrefab;

    private bool isFlipped = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         if (!isFlipped)
    //         {
    //             // 레버를 상하반전
    //             transform.Rotate(Vector3.up, 180.0f);
                
    //             // 블록 생성
    //             testPrefab.SetActive(false);
                
    //             isFlipped = true;
    //         }
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isFlipped)
            {
                transform.Rotate(Vector3.up, 180.0f);
                testPrefab.SetActive(false);
                isFlipped = true;
            }
        }
    }
}