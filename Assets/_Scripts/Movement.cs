using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    Rigidbody       rigid;
    bool collideToWall;

    float xAxis;
    [SerializeField] bool doJump;
    [SerializeField] bool isJump;

    void Start()
    {
        rigid       = GetComponent<Rigidbody>();
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

    public void Move()
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

    public void Jump()
    {
        if (!doJump) return;

        doJump = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // ===============================================================================================
    // 플랫폼의 수직면에 닿았는지 확인하는 함수
    // 플레이어에서 충돌점으로 향하는 벡터(collision.contacts[0].point - transform.position)와
    // (0, -1, 0) 벡터를 내적하여 플랫폼과 플레이어의 충돌 상태를 구함.
    // 0.75는 임의의 값. 추후 값 조정 예정
    // 올바른 충돌 시 SmoothJump 코루틴을 멈추고, 다시 점프할 수 있는 상태로 전환
    // 내적을 사용하여 계산한 이유 : collision.contacts[n].normal.y로 계산 시 플랫폼의 아래에서
    // 플레이어가 점프하여 정수리를 닿아도 올바른 충돌로 인식하기 때문에 오직 플레이어의 발로부터
    // 일정 각도의 충돌만 처리하기 위함
    // ===============================================================================================
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (Vector3.Dot(collision.contacts[0].point - transform.position, Vector3.down) > 0.75f)
            {
                isJump = false;
                StopCoroutine(SmoothJump());
            }
        }
    }

    // ===============================================================================================
    // 플랫폼의 벽 부분에 닿았는지 검사하는 함수
    // 한번이라도 벽에 충돌했다면 collideToWall 변수를 True로 만듦.
    // 반복문을 사용하여 검사하는 이유는 동시에 여러 물체와 충돌 상태가 될 수 있기 때문.
    // ===============================================================================================
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Platform" && !collideToWall)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector3.Dot(collision.contacts[i].point - transform.position, Vector3.down) <= 0.75f)
                {
                    collideToWall = true;
                    break;
                }
            }
        }
    }

    // ===============================================================================================
    // 플랫폼의 바닥에서 벗어날 때만 호출되는 함수
    // 벽에서 벗어났다면 호출되지 않으며, 플랫폼에서 점프하지 않고 미끄러 떨어진 상태라면 코루틴 실행하여
    // 일정 시간 뒤에 점프 불가 상태로 전환
    // ===============================================================================================
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform" && !collideToWall)
        {
            StartCoroutine(SmoothJump());
        }
    }

    // ===============================================================================================
    // 부드러운 점프 구현하기 위한 함수
    // 추후 함수에 있는 값 분리
    // ===============================================================================================
    private IEnumerator SmoothJump()
    {
        yield return new WaitForSeconds(0.2f);
        isJump = true;
    }
}
