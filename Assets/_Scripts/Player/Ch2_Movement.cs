using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    public GameObject   Clock;
    protected Animator  animator;
    private OxygenBar oxygenBar;
    public static Ch2_Movement instance;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
        oxygenBar = FindObjectOfType<OxygenBar>();
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    
    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !jumpByKey && jumpable)
        {
            animator.Play("Jumping");
            jumpByKey = true;
        }

        if (isTouchPlatform)
        {
            animator.SetBool("isGround", true);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                animator.Play("Running");
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            animator.SetBool("isGround", false);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                animator.Play("Swimming");
                animator.SetBool("isSwimming", true);
            }
            else
            {
                animator.SetBool("isSwimming", false);
            }
        }


        if (oxygenBar != null)
        {
            switch (oxygenBar.fillRatio * 5 + 1) //값 수정
            {
                case 1:
                    animator.SetInteger("Depletion", 1);
                    break;
                case 2:
                    animator.SetInteger("Depletion", 2);
                    break;
                case 3:
                    animator.SetInteger("Depletion", 3);
                    break;
                case 4:
                    animator.SetInteger("Depletion", 4);
                    break;
                case 5:
                    animator.SetInteger("Depletion", 5);
                    break;
            }
        }
    
    }
    


    private void FixedUpdate()
    {
        base.Move();
        Jump();
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

    IEnumerator waitForReloadClock()
    {
        yield return new WaitUntil(() => isTouchPlatform == true);
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
