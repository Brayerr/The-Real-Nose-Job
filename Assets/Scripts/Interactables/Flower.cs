using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactable
{
    public override void OnPlayerCollision(PlayerController controller)
    {
        controller.setIsOnFlower(true);
    }

    public override void OnPlayerExit(PlayerController controller)
    {
        controller.setIsOnFlower(false);
    }
}
