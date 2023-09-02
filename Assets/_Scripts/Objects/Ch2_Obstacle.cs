using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Obstacle : MonoBehaviour
{
    private float movementRange = 5f; 
    private float movementSpeed = 5f; 

    private Vector3 originPos;

    private GameObject player;
    private bool isTouched = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        originPos = transform.position;        
    }

    // 코루틴 ? 업데이트 ?
    private void Update()
    {
        transform.position = originPos + new Vector3(0, Mathf.Sin(Time.time) * 2.5f, 0);
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouched = true;
            player.gameObject.transform.position = new Vector3(-1, 0, 0);
        }
    }
}
