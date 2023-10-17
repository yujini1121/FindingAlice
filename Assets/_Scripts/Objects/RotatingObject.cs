using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] Vector3 rotationDirection = new Vector3(0, 1, 0);
    [SerializeField] float speed = 100;

    void Update()
    {
        Quaternion rotation = Quaternion.Euler(rotationDirection * (speed * Time.deltaTime));
        transform.localRotation *= rotation;
    }
}