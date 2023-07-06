using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    private enum ItemType
    {
        Clock,
        Oxygen
    }

    [SerializeField] private ItemType itemType;
    [SerializeField] private float respawnTime;

    Mesh objMesh;

    void Start()
    {
        objMesh = GetComponent<MeshFilter>().mesh;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshFilter>().mesh = null;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(Respawn());

            switch(itemType)
            {
                case ItemType.Clock:
                    ClockManager.instance.clockCounter++;
                    break;

                case ItemType.Oxygen:
                    break;
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshFilter>().mesh = objMesh;
        GetComponent<SphereCollider>().enabled = true;
    }
}
