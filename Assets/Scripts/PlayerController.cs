using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float risingVerticalSpeed;
    [SerializeField] float glidingVerticalSpeed;
    [SerializeField] float glidingSpeedMultiplier;

    float horizontal;

    [SerializeField] float maxSnortAmount;
    float currentSnortAmount;

    [SerializeField] float maxChargeAmount;
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
        float horizontalSpeedMod = hasBubble ? glidingSpeedMultiplier : 1;
        if (horizontal != 0) transform.position += new Vector3(horizontalSpeedMod * speed * horizontal * Time.deltaTime, 0, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded) ChargeBubble();

        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            reachedChargeAmount = currentChargeAmount; //Updating reachedCharge based on latest charging
            isCharging = false;
            isGrounded = false;
            hasBubble = true;
            isAscending = true;
        }

        if (hasBubble && isAscending) Jump();

        else if (hasBubble && !isAscending) Glide();

        GroundCheck();
    }

    void ChargeBubble()
    {
        isCharging = true;
        if (currentChargeAmount < maxChargeAmount) currentChargeAmount += chargingSpeed;
        Debug.Log($"Charging, {currentChargeAmount}");
        //can add reached max amount logic
    }

    void Jump()
    {
        float verticalSpeedMod = slowingUpward ? currentChargeAmount/reachedChargeAmount : 1; //Changed jump responsivnes. check if you like it.
        transform.position += new Vector3(0, verticalSpeedMod * risingVerticalSpeed * 1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f;
        if (currentChargeAmount < glidePrecentage * reachedChargeAmount) isAscending = false;
    }

    void Glide()
    {
        transform.position += new Vector3(0, glidingVerticalSpeed * -1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f; //Need to check with game design if during glide player loses charge
        if (isGrounded) CancelJump(); // after adding gravity we need to need add "|| currentChargeAmount == 0" in condition
    }

    void CancelJump()
    {
        currentChargeAmount = 0;
        hasBubble = false;
    }

    private void GroundCheck()
    {
        Debug.Log("Checking ground");
        //RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        //isGrounded = hitCenter.collider != null;
        if (Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), groundCheckDistance, groundLayer))
        {
            Debug.Log("Found ground");
            isGrounded = true;
        }
        else
        {
            Debug.Log("no ground");
            isGrounded = false;
        }
    }
}
