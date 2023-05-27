using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1_Movement : Movement
{
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            doJump = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }
}
