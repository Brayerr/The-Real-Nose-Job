using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    private void Start()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        transform.position += new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y, 0);
    }
}
