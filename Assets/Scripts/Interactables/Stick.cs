using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : Interactable
{
    public override void OnPlayerCollision(PlayerController controller)
    {
        //get stick
        Debug.Log("123231321");
        gameObject.SetActive(false);
        controller.pickUpBranch();
    }

    public override void OnPlayerExit(PlayerController controller)
    {

    }

}
