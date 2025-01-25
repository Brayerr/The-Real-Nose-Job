using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Interactable
{
    [SerializeField] int damage = 1;
    public override void OnPlayerCollision(PlayerController controller)
    {
        //take damage
        controller.GetComponentInParent<Health>().TakeDamage(damage);
    }

    public override void OnPlayerExit(PlayerController controller)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hi");
        other.GetComponent<BubbleController>().GetParentBubble().Explode();
    }
}
