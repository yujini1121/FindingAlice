using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private GameObject player;
    private Transform playerTf;
    private Rigidbody playerRb;
    private Queue<Vector3> playerPos;

    private Vector3 followPos;
    private float followSpeed;

    // ===============================================================================================
    // Queue
    // 
    // FIFO (First In First Out)
    // 먼저 입력된 데이터가 먼저 나가는 자료구조 (Stack과 정반대)
    // 
    // Enqueue : 큐에 데이터를 저장 (In)
    // Dequeue : 큐의 첫 데이터를 빼면서 반환 (Out)
    // ===============================================================================================

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        playerTf = player.GetComponent<Transform>();
        playerRb = player.GetComponent<Rigidbody>();
        playerPos = new Queue<Vector3>();
        followSpeed = 5f;
    }

    void Update()
    {
        Watch();
        Follow();
    }

    void Watch()
    {
        // Input Pos
        playerPos.Enqueue(player.transform.position);

        // Output Pos
        // 가장 먼저 저장된 값을 제거하고 그 값을 followPos에 저장
        if (playerPos.Count > followSpeed)
            followPos = playerPos.Dequeue();
        else if (playerPos.Count < followSpeed)
            followPos = playerTf.position;
    }

    // Watch()########################################
    // if (!playerPos.Contains(player.position))
    //     playerPos.Enqueue(player.position);

    // 제거된 값을 제거하고, 그 값을 followPos에 저장
    // if (playerPos.Count > followSpeed)
    //     followPos = playerPos.Dequeue();
    // else if (playerPos.Count < followSpeed)
    //     followPos = playerTf.position;


    void Follow()
    {
        transform.position = followPos;
    }
}
