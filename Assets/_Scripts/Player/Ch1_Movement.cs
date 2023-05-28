using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1_Movement : Movement
{
    void Update()
    {
        value.xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !value.isJump)
        {
            value.isJump = true;
            value.doJump = true;
        }
    }

    void FixedUpdate()
    {
        base.Move();
        base.Jump();
    }
}
