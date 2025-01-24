using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : Interactable
{
    public override void OnPlayerCollision(PlayerController controller)
    {
        //get stick
        gameObject.SetActive(false);
    }

    public override void OnPlayerExit(PlayerController controller)
    {

    }

}
