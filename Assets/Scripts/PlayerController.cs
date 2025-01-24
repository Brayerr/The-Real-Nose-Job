using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool canMove = true;
    [SerializeField] float risingVerticalSpeed;
    [SerializeField] float glidingVerticalSpeed;
    [SerializeField] float glidingSpeedMultiplier;

    float horizontal;
    float horizontalSpeedMod;

    [SerializeField] float maxSnortAmount;
    float currentSnortAmount;

    [SerializeField] float maxChargeAmount;
    [SerializeField] float minChargeAmount;
    [SerializeField] float chargingSpeed;
    [SerializeField] float glidePrecentage;
    [SerializeField] bool slowingUpward;
    float currentChargeAmount;
    float reachedChargeAmount; //In each seperate charge, player reaches a different charge amount.
                               //Gliding starting time is dependant on that amount.

    bool isGrounded = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;

    bool hasBubble;
    bool isCharging;
    bool isAscending;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        horizontalSpeedMod = hasBubble ? glidingSpeedMultiplier : 1;

        if (horizontal != 0 && canMove) Move();

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded) ChargeBubble();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            FinishCharging();
        }

        if (hasBubble)
        {
            if (isAscending) Jump();
            else Glide();
            if (Input.GetKeyDown(KeyCode.Space)) CancelJump();
        }


        GroundCheck();

        if (!hasBubble && !isGrounded) Fall();
    }

    void Move()
    {
        if (horizontal > 0)
        {
            //rotate right if not already rotated right
        }

        else
        {
            //rotate left if not already rotated left
        }

        transform.position += new Vector3(horizontalSpeedMod * speed * horizontal * Time.deltaTime, 0, 0);
    }

    void ChargeBubble()
    {
        isCharging = true;
        if (currentChargeAmount < maxChargeAmount) currentChargeAmount += chargingSpeed;
        
        //can add reached max amount logic
        if(currentChargeAmount >= maxChargeAmount)
        {
            currentChargeAmount = maxChargeAmount;
            FinishCharging();
        }
    }

    void FinishCharging()
    {
        //remove charge amount from snotMeter
        reachedChargeAmount = currentChargeAmount; //Updating reachedCharge based on latest charging
        if (reachedChargeAmount > minChargeAmount)
        {
            isCharging = false;
            isGrounded = false;
            hasBubble = true;
            isAscending = true;
        }
        else currentChargeAmount = 0;
    }

    void Jump()
    {
        float verticalSpeedMod = slowingUpward ? currentChargeAmount / reachedChargeAmount : 1; //Changed jump responsivnes. check if you like it.
        transform.position += new Vector3(0, verticalSpeedMod * risingVerticalSpeed * 1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f;
        if (currentChargeAmount < glidePrecentage * reachedChargeAmount) isAscending = false;
    }

    void Glide()
    {
        transform.position += new Vector3(0, glidingVerticalSpeed * -1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f; //Need to check with game design if during glide player loses charge
        if (isGrounded || currentChargeAmount == 0) CancelJump(); // after adding gravity we need to need add "|| currentChargeAmount == 0" in condition
    }

    void CancelJump()
    {
        currentChargeAmount = 0;
        hasBubble = false;
        Fall();
    }

    void Fall()
    {
        transform.position += new Vector3(0, -1 * (speed * 1.5f) * Time.deltaTime, 0);
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
