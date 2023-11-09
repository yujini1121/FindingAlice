using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCollection : MonoBehaviour
{

    [SerializeField] int bitFlagPlace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1을 bitFlagPlace만큼 시프트해서 데이터 저장
            DataController.instance.GetCollection(1 << bitFlagPlace);
            gameObject.SetActive(false); 
        }
    }
}