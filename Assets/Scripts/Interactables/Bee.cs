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
    [SerializeField] int damage;

    [SerializeField] PlayerController player;
    bool isStunned;
    bool isChasing = false;
    bool isForward = true;

    private void Start()
    {
        transform.position = PatrolStart.position;
    }

    private void Update()
    {
        if (!isStunned)
        {
            Transform currentDest = isForward ? PatrolEnd : PatrolStart;

            if (!isChasing)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentDest.position, moveSpeed * Time.deltaTime);
                if (currentDest.position == transform.position)
                {
                    isForward = !isForward;
                    currentDest = isForward ? PatrolEnd : PatrolStart;
                    transform.LookAt(currentDest);
                }
            }
            else
            {
                transform.LookAt(player.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            }
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

    public void SetStunned(bool stunned)
    {
        isStunned = stunned;
        Debug.Log("bee stunned");
    }

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
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("hit");
            SetStunned(true);
            Destroy(other.gameObject);
        }
    }
}
