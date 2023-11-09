using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    public GameObject   Clock;
    public static Ch2_Movement instance;
    public ClockManager clockManager;

    public float playerGravityModifier;

    protected void Awake()
    {

        animator = GetComponent<Animator>();
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        jumpForce = 12;
        moveSpeed = 4;
        playerGravityModifier = 20f;
        jumpable = true;
    }

    private void FixedUpdate()
    {
        base.Move();
        Jump();

        if (rigid.velocity.y < 0)
        {
            rigid.AddForce(Vector3.up * playerGravityModifier);
        }
    }

    private void Update()
    {
        base.Animator_Swim();
        base.Animator_Jump();
    }

    protected override void Jump()
    {
        if (!jumpByKey || !jumpable || isTalking) return;
        StartCoroutine(ResetJumpDelay());
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
    }

    IEnumerator ResetJumpDelay()
    {
        jumpable = false;
        yield return new WaitForSeconds(0.7f);
        jumpable = true;
        jumpByKey = false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isTouchPlatform = true;
            clockManager.NotifyIsTouchPlatform(isTouchPlatform);
            // 0.85f(Cos) ≒ 약 31.78도
            if (Vector3.Dot(collision.contacts[0].point - transform.position, Vector3.down) > 0.85f)
            {
                jumpByKey = false;
                jumpable = true;

                if (smoothJump != null)
                    StopCoroutine(smoothJump);
            }

            if (ClockManager.instance.clockCounter < 2)
            {
                ClockManager.instance.ClockCoroutineStart();
            }
        }
        
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isTouchPlatform = false;
            clockManager.NotifyIsTouchPlatform(isTouchPlatform);
            if (collideToWall)
                collideToWall = false;

            if (ClockManager.instance.clockCounter < 2)
            {
                ClockManager.instance.ClockCoroutinePause();
            }
        }


    }

    public void EnterRipCurrent(Vector3 vec)
    {
        if (!Clock.activeSelf)
        {
            rigid.velocity = Vector3.zero;
            rigid.transform.Translate(vec * Time.deltaTime);
            jumpable = false;
            movable = false;
            rigid.useGravity = false;
        }
    }

    public void ExitRipCurrent()
    {
        jumpable = true;
        movable = true;
        rigid.useGravity = true;
    }

}