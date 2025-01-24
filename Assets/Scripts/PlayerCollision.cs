using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        other.gameObject.GetComponent<Interactable>().OnPlayerCollision(controller);
        
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Interactable>().OnPlayerExit(controller);

    }

}
