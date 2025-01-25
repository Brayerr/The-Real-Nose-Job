using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollower : MonoBehaviour
{
    [Header("Anchor Settings")]
    [SerializeField] private Transform anchor;       // The pivot or "hinge" point
    [SerializeField] private float ropeLength = 2f;  // Distance from anchor (rod or rope length)

    [Header("Dynamics")]
    [SerializeField] private Vector3 velocity = Vector3.zero;  
    [Tooltip("How quickly the pendulum slows down. Set to 0 for no damping.")]
    [SerializeField] private float damping = 0.05f;  

    [Header("Initial Offset")]
    [Tooltip("Optional: set an initial local offset so the pendulum starts at an angle.")]
    [SerializeField] private Vector3 localStartOffset = new Vector3(1.5f, -1.5f, 0);

    private void Start()
    {
        // Place the object at the anchor + local offset so it starts away from the straight-down position
        transform.position = anchor.TransformPoint(localStartOffset);

        // OPTIONAL: Ensure ropeLength matches the magnitude of localStartOffset if you want exact distance
        // ropeLength = localStartOffset.magnitude;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1. Apply gravity to velocity
        velocity += Physics.gravity * dt;

        // 2. Integrate position
        transform.position += velocity * dt;

        // 3. Enforce the distance constraint
        Vector3 offset = transform.position - anchor.position;
        float dist = offset.magnitude;

        // Avoid degenerate case (anchor == object position)
        if (dist > Mathf.Epsilon)
        {
            // Constrain the position to ropeLength away from the anchor
            Vector3 dir = offset / dist;
            transform.position = anchor.position + dir * ropeLength;

            // Remove any radial velocity that would pull away or push toward the anchor
            // so the motion becomes purely tangential (like a rod pivot).
            float radialSpeed = Vector3.Dot(velocity, dir);
            Vector3 radialVel = radialSpeed * dir;
            velocity -= radialVel;
        }

        // 4. Damping (optional). Reduce velocity magnitude so it doesn't swing forever.
        // If you want perpetual swinging, set damping = 0. 
        velocity *= (1f - damping * dt);
    }
}