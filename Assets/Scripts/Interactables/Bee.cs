using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bee : Interactable
{
    [SerializeField] Transform PatrolStart;
    [SerializeField] Transform PatrolEnd;
    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] Health health;
    
    [SerializeField] PlayerController player;
    bool isStanned;
    bool isChasing = false;
    bool isForward = true;

    private void Start()
    {
        transform.position = PatrolStart.position;
    }

    private void Update()
    {
        Transform currentDest = isForward ? PatrolEnd : PatrolStart;

        if (!isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDest.position, moveSpeed*Time.deltaTime);
            if (currentDest.position == transform.position)
            {
                isForward = !isForward;
                currentDest = isForward ? PatrolEnd : PatrolStart;
                transform.LookAt(currentDest);
            }
        } else
        {
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
        }

    }

    public void NoticePlayerEnteredRange()
    {
        isChasing = true;
    }

    public void NoticePlayerExitedRange()
    {
        isChasing = false;
    }

    public override void OnPlayerCollision(PlayerController controller)
    {
        health.TakeDamage(1);
        //take damage
    }

    public override void OnPlayerExit(PlayerController controller)
    {

    }
}
