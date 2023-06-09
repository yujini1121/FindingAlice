using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1_Movement : Movement
{
    // ===============================================================================================
    // 점프 입력을 했을 때, 키에 의한 점프이지 않고, 점프할 수 있는 상태라면 jumpByKey를 true로 변경 -> Jump() 실행
    // ===============================================================================================
    private void Update()
    {

#if UNITY_EDITOR
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !jumpByKey && jumpable)
        {
            jumpByKey = true;
        }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
#endif
    }

    private void FixedUpdate()
    {
        base.Move();
        base.Jump();
    }
}
