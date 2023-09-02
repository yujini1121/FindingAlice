using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    public GameObject   Clock;
    public static Ch2_Movement instance;

    public float playerGravityModifier;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        jumpForce = 12;
        moveSpeed = 4;
        playerGravityModifier = 20f;
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

        jumpable = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        StartCoroutine(ResetJumpDelay());

    }

    IEnumerator ResetJumpDelay()
    {
        yield return new WaitForSeconds(0.2f);
        jumpable = true;
        jumpByKey = false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (ClockManager.instance.clockCounter < 2)
        {
            ClockManager.instance.ClockCoroutineStart();
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        if (ClockManager.instance.clockCounter < 2)
        {
            ClockManager.instance.ClockCoroutinePause();
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
