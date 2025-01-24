using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactable
{
    public override void OnPlayerCollision(PlayerController controller)
    {
        //enable recharge
    }

    public override void OnPlayerExit(PlayerController controller)
    {
        //disable recharge
    }
}
