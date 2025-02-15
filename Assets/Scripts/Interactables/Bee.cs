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
    [SerializeField] int damage;
    [SerializeField] private Animator _beeAnimator;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject stunBubble;

    bool isStunned;
    bool isChasing = false;
    bool isForward = true;

    private void Start()
    {
        transform.position = PatrolStart.position;
        stunBubble.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isStunned)
        {
            Transform currentDest = isForward ? PatrolEnd : PatrolStart;
            transform.LookAt(currentDest);
            if (!isChasing)
            {
                _beeAnimator.SetBool("isChasing",false);
                transform.position = Vector3.MoveTowards(transform.position, currentDest.position, moveSpeed * Time.deltaTime);
                if (currentDest.position == transform.position)
                {
                    isForward = !isForward;
                    currentDest = isForward ? PatrolEnd : PatrolStart;
                    
                }
            }
            else
            {
                _beeAnimator.SetBool("isChasing",true);
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
        Debug.Log("someone triggered bee");
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("it was booger");
            Destroy(other.gameObject);
            StartCoroutine(StunCoroutine());
        }
    }

    IEnumerator StunCoroutine()
    {
        SetStunned(true);
        stunBubble.SetActive(true);
        stunBubble.transform.eulerAngles = Vector3.zero;
        yield return new WaitForSeconds(3);
        SetStunned(false);
        stunBubble.SetActive(false);
    }
}
