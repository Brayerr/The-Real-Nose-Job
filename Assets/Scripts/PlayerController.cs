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
    [SerializeField] float glidePrecentage;
    float currentChargeAmount;

    bool isGrounded = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;

    bool hasBubble;
    bool isCharging;
    bool isAscending;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0) transform.position += new Vector3(speed * horizontal * Time.deltaTime, 0, 0);

        if(Input.GetKey(KeyCode.Space))
        {
            if(isGrounded) ChargeBubble();
            
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            isCharging = false;
            isGrounded = false;
            hasBubble = true;
            isAscending = true;
        }

        if (hasBubble && isAscending) Jump();

        else if (hasBubble && !isAscending) Glide();

        //GroundCheck();
    }

    void ChargeBubble()
    {
        isCharging = true;
        if (currentChargeAmount <  maxChargeAmount) currentChargeAmount += .1f;
        Debug.Log($"Charging, {currentChargeAmount}");
        //can add reached max amount logic
    }

    void Jump()
    {
        transform.position += new Vector3(0, risingVerticalSpeed * 1 * Time.deltaTime, 0);
        currentChargeAmount -= .1f;
        if (currentChargeAmount < glidePrecentage * maxChargeAmount) isAscending = false;
    }

    void Glide()
    {
        transform.position += new Vector3(0, glidingVerticalSpeed * -1 * Time.deltaTime, 0);
        if (isGrounded) CancelJump();
    }

    void CancelJump()
    {
        hasBubble = false;
    }

    private void GroundCheck()
    {
        Debug.Log("Checking ground");
        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        //isGrounded = hitCenter.collider != null;
        if( hitCenter.collider != null )
        {
            Debug.Log("Found ground");
            isGrounded = true;
        }
        else
        {
            Debug.Log($"no ground {hitCenter}");
            isGrounded = false;
        }
    }
}
