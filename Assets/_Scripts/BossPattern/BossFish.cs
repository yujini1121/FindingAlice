using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFish : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;   
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            bossFish();
        }
    }

    private IEnumerator bossFish()
    {
        GameManager.instance.PlayerDead();

        yield return null;
    }
}