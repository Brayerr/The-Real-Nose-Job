using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDetection : Interactable
{
    [SerializeField] Bee bee;
    public override void OnPlayerCollision(PlayerController controller)
    {
        bee.NoticePlayerEnteredRange();
    }

    public override void OnPlayerExit(PlayerController controller)
    {
        bee.NoticePlayerExitedRange();
    }
}
