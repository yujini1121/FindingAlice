using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    Rigidbody       rigid;
    CapsuleCollider capCollider;

    float xAxis;
    [SerializeField] bool doJump;
    [SerializeField] bool isJump;

    void Start()
    {
        rigid       = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
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

    void Move()
    {
        if (transform.localScale.x < 0 && xAxis > 0)
        {
            Vector3 reverse = transform.localScale;
            reverse.x = -transform.localScale.x;
            transform.localScale = reverse;
        }
        if (transform.localScale.x > 0 && xAxis < 0)
        {
            Vector3 reverse = transform.localScale;
            reverse.x = -transform.localScale.x;
            transform.localScale = reverse;
        }

        Vector3 velocity = new Vector3(xAxis, 0, 0);
        velocity = velocity * moveSpeed;

        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
    }

    void Jump()
    {
        if (!doJump) return;

        doJump = false;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (collision.contacts[i].normal.y > 0.4f)
                {
                    isJump = false;
                    break;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (collision.contacts[0].normal.y < 0.4f)
                return;

            StartCoroutine(SmoothJump());
        }
    }

    IEnumerator SmoothJump()
    {
        yield return new WaitForSeconds(0.2f);
        isJump = true;
    }
}
