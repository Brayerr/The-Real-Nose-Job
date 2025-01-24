using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    abstract public void OnPlayerCollision(PlayerController controller);
    abstract public void OnPlayerExit(PlayerController controller);
}
