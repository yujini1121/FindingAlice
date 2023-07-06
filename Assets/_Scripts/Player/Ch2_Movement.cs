using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    private bool canJump = true;
    
    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !jumpByKey && jumpable)
        {
            jumpByKey = true;
        }
    }

    private void FixedUpdate()
    {
        base.Move();
        Jump();
    }

    protected override void Jump()
    {
        if (!jumpByKey || !jumpable) return;

        jumpable = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Debug.Log(!jumpByKey + " " + !jumpable);

        StartCoroutine(ResetJumpDelay());
    }

    IEnumerator ResetJumpDelay()
    {
        yield return new WaitForSeconds(1f);
        jumpable = true;
        jumpByKey = false;
    }
}
