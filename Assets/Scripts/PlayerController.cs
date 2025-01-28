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
    [SerializeField] public UnityEvent onJumpCanceled;
    [SerializeField] public UnityEvent<int> onBranchAmountChange;

    [SerializeField] Animator animator;
    [SerializeField] BubbleCreator bubbleCreator;
    [SerializeField] public ParticleSystemForceField sniffForceField;
    [SerializeField] ParticleSystem runningDust;
    [SerializeField] Flower currentFlower;
    Transform spawnLoc;
    SpringFollower spring;

    [SerializeField] float speed = 5f;
    [SerializeField] bool canMove = true;
    [SerializeField] float risingVerticalSpeed;
    [SerializeField] float glidingVerticalSpeed;
    [SerializeField] float glidingSpeedMultiplier;
    Vector3 glideVerticalDelta;

    public float horizontal;
    public float lastHorizontal;
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
    float reachedChargeAmount;

    bool isGrounded = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] Vector3 groundCheckPosOffset = new(0, -0.1f, 0);
    [SerializeField] Vector3 leftCheckPosOffset = new(-0.5f, 0, 0);
    [SerializeField] Vector3 rightCheckPosOffset = new(0.5f, 0, 0);

    bool hasBubble;
    bool isCharging;
    bool canCharge = true;
    bool isAscending;
    bool isOnFlower;
    private bool isTouchingFromTop;
    private bool isTouchingFromLeft;
    private bool isTouchingFromRight;
    private int branchAmount = 0;

    [SerializeField] float sideTurnSpeed = 5f;

    // New variables for acceleration/deceleration
    [SerializeField] float acceleration = 10f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float currentXSpeed = 0f;

    // New variables for turn blending
    [SerializeField] float turnBlendSpeed = 5f;
    float turnBlend = 0f;

    private void Awake()
    {
        Bubble.OnBubbleExploded += CancelJump;
    }

    private void OnDestroy()
    {
        Bubble.OnBubbleExploded -= CancelJump;
    }

    private void Start()
    {
        spring = GetComponent<SpringFollower>();
        spawnLoc = Instantiate(new GameObject().transform);
        bubbleCreator.SetSpawnLocation(spawnLoc);
        spring.SetAnchor(spawnLoc);
    }

    public void setIsOnFlower(bool b)
    {
        isOnFlower = b;
    }

    public float getMinChargeAmount()
    {
        return minChargeAmount;
    }

    public float getHorizontal()
    {
        return horizontal;
    }

    public float getCurrentSnotAmount()
    {
        return currentSnotAmount;
    }

    public float getMaxSnotAmount()
    {
        return maxSnotAmount;
    }

    public int getBranchAmount()
    {
        return branchAmount;
    }

    public void SetCanMove(bool b)
    {
        canMove = b;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0) lastHorizontal = 1;
        else if (horizontal < 0) lastHorizontal = -1;

        horizontalSpeedMod = hasBubble ? glidingSpeedMultiplier : 1;

        if ((horizontal != 0 || currentXSpeed != 0) && canMove && !isCharging)
        {
            Move();
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.speed = 1;
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bubbleCreator.CreateBubble();
            }
            if (Input.GetKey(KeyCode.Space) && canCharge)
            {
                ChargeBubble();
            }

            if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            {
                FinishCharging();
            }
            if (Input.GetKey(KeyCode.LeftControl) && !isCharging)
            {
                Sniff();
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                animator.SetBool("isSniffing", false);
                sniffForceField.endRange = 0;
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
        animator.SetBool("isRunning", true);
        animator.speed = 1.5f;

        Quaternion targetRotation;

        if (horizontal > 0)
        {
            animator.SetFloat("Blend", horizontal);

            // Target rotation when facing right
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal < 0)
        {
            animator.SetFloat("Blend", -horizontal);

            // Target rotation when facing left
            targetRotation = Quaternion.Euler(0, -180, 0);
        }
        else targetRotation = transform.rotation;

        if (currentXSpeed > 0)
        {
            if (isTouchingFromRight)
            {
                currentXSpeed = 0;
                print("touching from right");
                return;
            }
        }
        else if (currentXSpeed < 0)
        {
            if (isTouchingFromLeft)
            {
                currentXSpeed = 0;
                print("touching from left");
                return;
            }
        }

        // Rotate smoothly toward target
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            sideTurnSpeed * Time.deltaTime
        );


        // measure how far from the target rotation we are, then move turnBlend in or out
        float rotationDiff = Quaternion.Angle(transform.rotation, targetRotation);
        if (rotationDiff > 0.1f)
        {
            // qe are actively turning
            turnBlend = Mathf.MoveTowards(turnBlend, 1f, turnBlendSpeed * Time.deltaTime);
        }
        else
        {
            // close enough to target rotation, blend back
            turnBlend = Mathf.MoveTowards(turnBlend, 0f, turnBlendSpeed * Time.deltaTime);
        }
        //animator.SetFloat("TurnBlend", turnBlend);
        // ------------------------

        // Apply acceleration/deceleration
        float targetSpeed = horizontal * speed;
        float accel = (Mathf.Abs(horizontal) > 0.01f) ? acceleration : deceleration;
        currentXSpeed = Mathf.MoveTowards(currentXSpeed, targetSpeed, accel * Time.deltaTime);

        if (hasBubble)
        {
            spring.anchor.transform.position += new Vector3(horizontalSpeedMod * currentXSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += new Vector3(horizontalSpeedMod * currentXSpeed * Time.deltaTime, 0, 0);
        }
    }

    void ChargeBubble()
    {
        isCharging = true;
        currentXSpeed = 0;
        animator.SetBool("isCharging", true);
        if (currentChargeAmount < maxChargeAmount && currentChargeAmount < currentSnotAmount)
        {
            currentChargeAmount += chargingSpeed * Time.deltaTime;
            bubbleCreator.ExpandBubble();
        }
        else
        {
            currentChargeAmount = Mathf.Min(maxChargeAmount, currentSnotAmount);
        }
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
    }

    void FinishCharging()
    {
        reachedChargeAmount = currentChargeAmount;
        if (reachedChargeAmount > minChargeAmount)
        {
            currentSnotAmount -= reachedChargeAmount;
            isCharging = false;
            isGrounded = false;
            hasBubble = true;
            spring.isAnchorFollowing = false;
            isAscending = true;
            glideVerticalDelta = Vector3.zero;
            onSniffingChanged?.Invoke(currentSnotAmount, maxSnotAmount);
            spring.SetAnchor(bubbleCreator.LaunchBubble());
        }
        else
        {
            isCharging = false;
            currentChargeAmount = 0;
            onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
            bubbleCreator.AbortBubble();
            animator.SetBool("isCharging", false);
        }
    }

    void Jump()
    {
        float verticalSpeedMod = slowingUpward ? currentChargeAmount / reachedChargeAmount : 1;
        if (!CeilingCheck()) spring.anchor.transform.position += new Vector3(0, verticalSpeedMod * risingVerticalSpeed * Time.deltaTime, 0);
        currentChargeAmount -= 10f * Time.deltaTime;
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        if (currentChargeAmount < glidePrecentage * reachedChargeAmount) isAscending = false;
    }

    void Glide()
    {
        glideVerticalDelta += glidingVerticalSpeed * Time.deltaTime * Vector3.down;
        spring.anchor.transform.position += glideVerticalDelta;
        currentChargeAmount -= 10f * Time.deltaTime;
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        if (isGrounded || currentChargeAmount <= 0) CancelJump();
    }

    void CancelJump()
    {
        reachedChargeAmount = 0;
        currentChargeAmount = 0;
        onChargeAmountChanged.Invoke(currentChargeAmount, maxChargeAmount);
        hasBubble = false;
        spring.isAnchorFollowing = true;
        spring.SetAnchor(spawnLoc);
        isAscending = false;
        bubbleCreator.AbortBubble();
        animator.SetBool("isCharging", false);
        //Fall();
    }

    void Fall()
    {
        print("falling");
        transform.position += new Vector3(0, -1 * (speed * 1.5f) * Time.deltaTime, 0);
    }

    void Sniff()
    {
        if (isOnFlower && currentSnotAmount < maxSnotAmount)
        {
            animator.SetBool("isSniffing", true);
            //sniffParticle
            if(currentFlower != null) sniffForceField.endRange = 1.5f;            
            currentSnotAmount += sniffingSpeed * Time.deltaTime;
            onSniffingChanged?.Invoke(currentSnotAmount, maxSnotAmount);
        }
    }

    public void pickUpBranch()
    {
        branchAmount++;
        onBranchAmountChange.Invoke(branchAmount);
    }

    private bool CeilingCheck()
    {
        return Physics.CheckSphere(transform.position + Vector3.up * 2, groundCheckDistance, groundLayer);
    }

    private void GroundCheck()
    {
        if (!isAscending && Physics.CheckSphere(transform.position + groundCheckPosOffset, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
            runningDust.gameObject.SetActive(true);
        }
        else
        {
            isGrounded = false;
            runningDust.gameObject.SetActive(false);
        }

        if (Physics.CheckSphere(transform.position + leftCheckPosOffset, groundCheckDistance, groundLayer))
        {
            isTouchingFromLeft = true;
        }
        else
        {
            isTouchingFromLeft = false;
        }

        if (Physics.CheckSphere(transform.position + rightCheckPosOffset, groundCheckDistance, groundLayer))
        {
            isTouchingFromRight = true;
        }
        else
        {
            isTouchingFromRight = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + rightCheckPosOffset, groundCheckDistance);
        Gizmos.DrawSphere(transform.position + leftCheckPosOffset, groundCheckDistance);
        Gizmos.DrawSphere(transform.position + groundCheckPosOffset, groundCheckDistance);
    }

    public void SetCurrentFlower(Flower flower)
    {
        currentFlower = flower;
    }
}
