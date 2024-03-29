using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 챕터1 Player에 Attach되는 스크립트
//
// Movement 클래스를 상속받는다.
// ===================================================================================================

public class Ch1_Movement : Movement
{
    // ===============================================================================================
    // 점프 입력을 했을 때, 키에 의한 점프이지 않고, 점프할 수 있는 상태라면
    // JumpInput 함수 호출
    // ===============================================================================================

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        base.Animator_Run();
        base.Animator_Jump();
    }

    private void FixedUpdate()
    {
        base.Move();
        base.Jump();
    }
}
