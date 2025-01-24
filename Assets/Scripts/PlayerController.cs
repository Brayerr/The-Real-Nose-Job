using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public UnityEvent<float, float> onChargeAmountChanged;
    [SerializeField] public UnityEvent<float, float> onSniffingChanged;

    [SerializeField] float speed = 5f;
    [SerializeField] bool canMove = true;
    [SerializeField] float risingVerticalSpeed;
    [SerializeField] float glidingVerticalSpeed;
    [SerializeField] float glidingSpeedMultiplier;

    float horizontal;
    float horizontalSpeedMod;

    [SerializeField] float maxSnotAmount;
    [SerializeField] float sniffingSpeed;
    float currentSnotAmount = 0;

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
    bool canCharge = true;
    bool isAscending;
    bool isOnFlower;

    public void setIsOnFlower(bool b) { 
        isOnFlower = b;
    }

    public float getMinChargeAmount()
    {
        return minChargeAmount;
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        horizontalSpeedMod = hasBubble ? glidingSpeedMultiplier : 1;

        if (horizontal != 0 && canMove && !isCharging) Move();

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space) && canCharge) ChargeBubble();

            if (Input.GetKeyUp(KeyCode.Space) && isCharging) FinishCharging();
            if (Input.GetKey(KeyCode.LeftControl) && !isCharging)
            {
                Sniff();
            }
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
        if (currentChargeAmount < maxChargeAmount)
        {
            currentChargeAmount += chargingSpeed;
            onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        }

        //can add reached max amount logic
        if (currentChargeAmount >= maxChargeAmount || currentChargeAmount >= currentSnotAmount)
        {
            currentChargeAmount = maxChargeAmount;
            onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
            FinishCharging();
        }
    }

    void FinishCharging()
    {
        //remove charge amount from snotMeter
        reachedChargeAmount = currentChargeAmount; //Updating reachedCharge based on latest charging
        if (reachedChargeAmount > minChargeAmount && reachedChargeAmount <= currentSnotAmount)
        {
            Debug.Log($"releasing charge {reachedChargeAmount}, current snot {currentSnotAmount}");
            currentSnotAmount -= reachedChargeAmount;
            isCharging = false;
            isGrounded = false;
            hasBubble = true;
            isAscending = true;
            onSniffingChanged?.Invoke(currentSnotAmount, maxSnotAmount);
        }
        else
        {
            Debug.Log($"Failed charge {reachedChargeAmount}, current snot {currentSnotAmount}");

            isCharging = false;
            currentChargeAmount = 0;
            onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        }
    }

    void Jump()
    {
        float verticalSpeedMod = slowingUpward ? currentChargeAmount / reachedChargeAmount : 1; //Changed jump responsivnes. check if you like it.
        transform.position += new Vector3(0, verticalSpeedMod * risingVerticalSpeed * 1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f;
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        if (currentChargeAmount < glidePrecentage * reachedChargeAmount) isAscending = false;
    }

    void Glide()
    {
        transform.position += new Vector3(0, glidingVerticalSpeed * -1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f; //Need to check with game design if during glide player loses charge
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        if (isGrounded || currentChargeAmount == 0) CancelJump(); // after adding gravity we need to need add "|| currentChargeAmount == 0" in condition
    }

    void CancelJump()
    {
        reachedChargeAmount = 0;
        currentChargeAmount = 0;
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        hasBubble = false;
        isAscending = false;
        Fall();
    }

    void Fall()
    {
        transform.position += new Vector3(0, -1 * (speed * 1.5f) * Time.deltaTime, 0);
    }

    void Sniff()
    {
        if (isOnFlower && currentSnotAmount < maxSnotAmount)
        {
            currentSnotAmount += sniffingSpeed;
            onSniffingChanged?.Invoke(currentSnotAmount, maxSnotAmount);
        }
    }

    private void GroundCheck()
    {
        if (!isAscending && Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}