using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log("왜 안 찍혀");
        Vector3.Lerp(player.transform.position, transform.position, 0.5f);
    }
}
