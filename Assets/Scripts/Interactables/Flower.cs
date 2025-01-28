using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactable
{
    [SerializeField] public ParticleSystem particleSystem;
    public override void OnPlayerCollision(PlayerController controller)
    {
        controller.setIsOnFlower(true);
        controller.SetCurrentFlower(this);
        particleSystem.externalForces.AddInfluence(controller.sniffForceField);
    }

    public override void OnPlayerExit(PlayerController controller)
    {
        controller.setIsOnFlower(false);
        particleSystem.externalForces.RemoveInfluence(controller.sniffForceField);
        controller.SetCurrentFlower(null);
    }

}
