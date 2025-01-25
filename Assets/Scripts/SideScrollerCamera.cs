using UnityEngine;

public class SideScrollerCamera : MonoBehaviour
{
    [Header("Target & Basic Settings")]
    [SerializeField] private Transform target;    // The main target to follow (e.g. player)
    [SerializeField] private Vector2 offset;      // Offset from target in X and Y
    [SerializeField] private float followSpeed = 5f;

    [Header("Smoothing/Ease")]
    [SerializeField] private float smoothTime = 0.2f; 
    // Higher means more delay (so it's smoother but more lag)
    // Lower means snappier camera movement

    [Header("Lookahead / Lead")]
    [SerializeField] private bool enableLookahead;
    [SerializeField] private float lookaheadDistance = 2f; 
    [SerializeField] private float lookaheadSpeed = 3f; 

    [Header("Interest Override")]
    [SerializeField] private Transform interestPoint; 
    [SerializeField] private bool overrideActive;
    [SerializeField] private float overrideDuration = 2f;
    private float overrideTimer;

    [Header("Misc")]
    [SerializeField] private bool limitVerticalMovement; 
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    private Vector3 _currentVelocity;
    private float _currentLookahead;
    private Vector3 _targetPos;

    private void LateUpdate()
    {
        if (!target) return;
        
        // 1) Check if interest override is active
        if (overrideActive && interestPoint != null && overrideTimer < overrideDuration)
        {
            _targetPos = interestPoint.position;
            overrideTimer += Time.deltaTime;
        }
        else
        {
            // Once we exceed override duration or don't have an interest point
            overrideActive = false;
            _targetPos = target.position;
        }

        // 2) Calculate offset
        Vector3 desiredPos = _targetPos + (Vector3)offset;

        // 3) Optional lookahead (leading in the x direction)
        if (enableLookahead)
        {
            float targetLookahead = (target.localScale.x > 0) ? lookaheadDistance : -lookaheadDistance;
            _currentLookahead = Mathf.Lerp(_currentLookahead, targetLookahead, lookaheadSpeed * Time.deltaTime);
            desiredPos.x += _currentLookahead;
        }

        // 4) Smoothly move camera from current pos toward desired pos
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref _currentVelocity, smoothTime, followSpeed);

        // 5) Optionally limit vertical movement
        if (limitVerticalMovement)
        {
            smoothedPos.y = Mathf.Clamp(smoothedPos.y, minY, maxY);
        }

        // 6) Preserve the camera's own Z position
        smoothedPos.z = transform.position.z;

        // 7) Assign the final position
        transform.position = smoothedPos;
    }

    /// <summary>
    /// Triggers an interest override manually, resetting override timer.
    /// </summary>
    public void TriggerInterestOverride(Transform newInterest, float duration)
    {
        interestPoint = newInterest;
        overrideDuration = duration;
        overrideTimer = 0f;
        overrideActive = true;
    }
}
