using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    public GameObject               Clock;
    public static Ch2_Movement      instance;
    public ClockManager             clockManager;

    public float                    playerGravityModifier;

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
        Debug.Log(playerGravityModifier);
        Move();
        Jump();

        if (rigid.velocity.y < 0)
        {
            // 플레이어가 하강할 때, (0, 20, 0)만큼 AddForce
            rigid.AddForce(Vector3.up * playerGravityModifier);
        }
    }

    protected override void Move()
    {
        float oxygenRatio = OxygenBar.instance.OxygenRatio;
        animator.SetFloat("Depletion", oxygenRatio);
#if UNITY_EDITOR
        xAxis = Input.GetAxisRaw("Horizontal");
        if (isTouchPlatform) { animator.SetBool("isRecoiling", false); }
        animator.SetBool("isSwimming", xAxis != 0);
#elif UNITY_ANDROID
        xAxis = joystick.Horizontal;
        animator.SetBool("isSwimming", xAxis != 0);
#endif
        animator.SetBool("isDropping", rigid.velocity.y < -0.9);

        if (!movable) return;

        // 좌우 이동에 따라 플레이어 Flip
        if ((transform.localScale.x < 0 && xAxis > 0) || (transform.localScale.x > 0 && xAxis < 0))
        {
            Vector3 reverse = transform.localScale;
            reverse.x = -transform.localScale.x;
            transform.localScale = reverse;
        }

        Vector3 velocity = new Vector3(xAxis, 0, 0);

        // 추후 수정 필요
        if (Mathf.Abs(vecClockFollow.x) > moveSpeed)
        {
            velocity.x = vecClockFollow.x;
            vecClockFollow *= speedDecreaseRate;
        }
        else
        {
            velocity *= moveSpeed;
        }
        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, 0);
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