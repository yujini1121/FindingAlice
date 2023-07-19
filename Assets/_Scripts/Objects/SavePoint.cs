using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// Save Point에 Attach되는 스크립트
//
// Inspector 창에서 SavePoint Collider의 크기를 지정해두면 콜라이더의 위치가 조정된다.
// (Tranform Position에서 좌표를 어렵게 지정하지 않아도 되도록)
// ===================================================================================================

public class SavePoint : MonoBehaviour
{
    // 모든 SavePoint는 서로 다른 bitFlagPlace 값을 가지고 있어야 한다. 정수(0 ~ )
    [SerializeField] int bitFlagPlace;

    [Header("Bounds")]
    [SerializeField] private float SavePointScaleX;
    [SerializeField] private float SavePointScaleY;
    [SerializeField] private float SavePointScaleZ;

    private void Start()
    {
        GetComponent<BoxCollider>().center  = new Vector3(0, SavePointScaleY / 2 + 0.5f, 0);
        GetComponent<BoxCollider>().size    = new Vector3(SavePointScaleX, SavePointScaleY, SavePointScaleZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1을 bitFlagPlace만큼 시프트해서 저장 함수 호출
            DataController.instance.SaveData(1 << bitFlagPlace);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x,
                                        transform.position.y + (SavePointScaleY / 2 + 0.5f),
                                        transform.position.z),
                            new Vector3(SavePointScaleX, SavePointScaleY, SavePointScaleZ));
    }
}
