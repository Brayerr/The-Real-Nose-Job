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
        try
        {
            other.gameObject.GetComponent<Interactable>().OnPlayerCollision(controller);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        try
        {
            other.gameObject.GetComponent<Interactable>().OnPlayerExit(controller);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
    }

}
