using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bee : Interactable
{
    [SerializeField] Transform PatrolStart;
    [SerializeField] Transform PatrolEnd;
    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;

    PlayerController player;
    bool isPlayerInRange;
    bool isChasing;

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (isPlayerInRange)//and player has snot
            {
                isChasing = true;
            }
            else
            {
                StopChasing();
            }
        }
    }

    public void NoticePlayerEnteredRange()
    {
        isPlayerInRange = true;
    }

    public void NoticePlayerExitedRange()
    {
        isPlayerInRange = false;
        StopChasing();
    }

    public void StopChasing()
    {
        isChasing = false;
    }

    public override void OnPlayerCollision(PlayerController controller)
    {
        //take damage
    }

    public override void OnPlayerExit(PlayerController controller)
    {

    }
}
