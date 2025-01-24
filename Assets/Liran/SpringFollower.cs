using UnityEngine;

public class SpringFollower : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private Transform anchor;          // Where the pendulum is “attached”
    [SerializeField] private float desiredDistance = 2f;// Distance (rod length) from anchor
    [SerializeField] private float damping = 0.1f;      // How quickly it slows down
    [SerializeField] private Vector3 velocity = Vector3.zero; // Current velocity

    // Optional: if you want a custom "rest offset" (i.e. it doesn't hang straight down),
    // set this to a local offset from the anchor in Start() or via Inspector.
    [SerializeField] private Vector3 localRestOffset = new Vector3(0, -2, 0);

    private void Start()
    {
        // Place the object at the anchor + some local offset as an initial position
        // so it starts in the "rest" position you want.
        // The magnitude of localRestOffset should match desiredDistance,
        // but it’s okay to adjust or enforce below.
        transform.position = anchor.TransformPoint(localRestOffset);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1. Apply "gravity" (or any other acceleration you want).
        //    (Physics.gravity is typically (0, -9.81f, 0).)
        velocity += Physics.gravity * dt;

        // 2. Integrate position with the current velocity.
        transform.position += velocity * dt;

        // 3. Enforce the distance constraint to keep it “hinged” at a fixed length.
        //    This simulates a rod of length desiredDistance from anchor to object.
        Vector3 offset = transform.position - anchor.position;
        float currentDistance = offset.magnitude;

        // Avoid dividing by zero if somehow anchor and object coincide exactly:
        if (currentDistance > Mathf.Epsilon)
        {
            // Re-clamp the position so that the distance is exactly desiredDistance.
            Vector3 correctedOffset = offset.normalized * desiredDistance;
            transform.position = anchor.position + correctedOffset;
        }

        // 4. Apply damping to slow the swing over time.
        //    One way is to scale the velocity each frame:
        velocity *= (1f - damping * dt);

        // That’s it. The object will “dangle” from the anchor with approximate pendulum motion.
    }
}
