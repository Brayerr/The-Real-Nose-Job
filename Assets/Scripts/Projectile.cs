using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float horizontal;
    [SerializeField] float speed;

    private void Start()
    {
        if (horizontal == 0) horizontal = 1;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
    }
}
