using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxAngularVelocity = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float pullSpeed = 2f;
    [SerializeField] private float pullRange = 2f;
    [SerializeField] private float interactionRange = 1.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Gravidade personalizada")]
    [SerializeField] private float customGravityScale = 2f;
    [SerializeField] private float maxFallSpeed = -8f;

    [Header("Movimento no ar")]
    [SerializeField] private float airControlForce = 5f;
    [SerializeField] private float maxAirHorizontalSpeed = 6f;

    [Header("Pulo variável")]
    [SerializeField] private float maxJumpHoldTime = 0.3f;
    [SerializeField] private float extraJumpForce = 20f;

    [Header("Controles")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button interactButton;

    public static bool IsInPuzzle = false;
    private Rigidbody rb;
    private bool isGrounded = false;
    private GameObject pulledObject = null;
    private Rigidbody pulledObjectRb = null;
    private bool isHoldingInteract = false;
    private bool isPulling = false;

    private PuzzleInteractable currentPuzzle;
    private CollectibleItem nearbyCollectible;
    private InteractionFeedback currentNearbyFeedback;

    private float groundedCooldown = 0.2f;
    private float timeSinceJump = 0f;

    private float defaultAngularDrag;
    private float stoppedAngularDrag = 10f;
    private float inputThreshold = 0.1f;

    private float jumpCooldown = 0.3f;
    private float lastJumpTime = -1f;

    private bool isJumpButtonHeld = false;
    private bool canJumpAgain = true;
    private bool isJumping = false;
    private float jumpHoldTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        defaultAngularDrag = rb.angularDamping;
        rb.maxAngularVelocity = maxAngularVelocity;

        var jumpTrigger = jumpButton.gameObject.AddComponent<EventTrigger>();

        var jumpPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        jumpPointerDown.callback.AddListener((data) =>
        {
            if (!isJumpButtonHeld && canJumpAgain)
            {
                isJumpButtonHeld = true;
                Jump();
            }
        });

        var jumpPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        jumpPointerUp.callback.AddListener((data) =>
        {
            isJumpButtonHeld = false;
            isJumping = false;
        });

        jumpTrigger.triggers.Add(jumpPointerDown);
        jumpTrigger.triggers.Add(jumpPointerUp);

        var trigger = interactButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => { OnInteractButtonPressed(); });

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => { OnInteractButtonReleased(); });

        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);

        if (GameState.ShouldLoad)
        {
            Debug.Log("[LOAD] GameState.ShouldLoad é true, chamando LoadGame() via PlayerController");
            GameState.ShouldLoad = false;
            SaveSystem.LoadGame(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (IsInPuzzle) return;

        timeSinceJump += Time.fixedDeltaTime;
        CheckGrounded();

        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        if (input.magnitude > 1f) input.Normalize();

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * input.y + cameraRight * input.x).normalized;

        float currentMoveSpeed = isPulling ? moveSpeed * 0.6f :
                                 !isGrounded ? moveSpeed * 1.2f :
                                 moveSpeed;

        bool hasInput = input.magnitude > inputThreshold;

        if (hasInput)
        {
            Vector3 torqueDirection = new Vector3(moveDirection.z, 0, -moveDirection.x);
            rb.AddTorque(torqueDirection * currentMoveSpeed, ForceMode.VelocityChange);
            rb.angularDamping = isGrounded ? defaultAngularDrag : 0.05f;
        }
        else
        {
            rb.angularDamping = isGrounded ? stoppedAngularDrag : 0.05f;
        }

        if (!isGrounded && hasInput)
        {
            Vector3 airForce = moveDirection * airControlForce;
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (horizontalVelocity.magnitude < maxAirHorizontalSpeed)
            {
                rb.AddForce(airForce, ForceMode.Acceleration);
            }
        }

        if (isJumping && isJumpButtonHeld && jumpHoldTimer < maxJumpHoldTime)
        {
            float forceThisFrame = extraJumpForce * Time.fixedDeltaTime;
            rb.AddForce(Vector3.up * forceThisFrame, ForceMode.VelocityChange);
            jumpHoldTimer += Time.fixedDeltaTime;
        }

        if (isHoldingInteract)
        {
            if (pulledObject == null)
            {
                StartPulling();
            }
            else
            {
                Vector3 pullDirection = moveDirection;
                if (pullDirection.magnitude > 0.1f)
                {
                    Vector3 targetPosition = pulledObjectRb.position + pullDirection * pullSpeed * Time.fixedDeltaTime;
                    pulledObjectRb.MovePosition(Vector3.Lerp(pulledObjectRb.position, targetPosition, 0.2f));
                }
            }
        }
        else if (pulledObject != null)
        {
            pulledObject = null;
            pulledObjectRb = null;
            isPulling = false;
        }

        DetectNearbyFeedback();
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        if (!isGrounded)
        {
            Vector3 gravity = Physics.gravity * customGravityScale;
            rb.AddForce(gravity, ForceMode.Acceleration);

            if (rb.linearVelocity.y < maxFallSpeed)
            {
                Vector3 v = rb.linearVelocity;
                v.y = maxFallSpeed;
                rb.linearVelocity = v;
            }
        }
    }

    public void Jump()
    {
        if (isGrounded && !isPulling && (Time.time - lastJumpTime) > jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            timeSinceJump = 0f;
            lastJumpTime = Time.time;
            canJumpAgain = false;
            isJumping = true;
            jumpHoldTimer = 0f;
            Debug.Log("Jump executado!");
        }
    }

    private void OnInteractButtonPressed()
    {
        isHoldingInteract = true;

        if (nearbyCollectible != null)
        {
            nearbyCollectible.Coletar();
            nearbyCollectible = null;
            return;
        }

        if (currentPuzzle != null)
        {
            currentPuzzle.Interact();
            return;
        }

        if (pulledObject == null)
        {
            StartPulling();
        }
    }

    private void OnInteractButtonReleased()
    {
        isHoldingInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectibleItem item))
        {
            nearbyCollectible = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CollectibleItem item) && item == nearbyCollectible)
        {
            nearbyCollectible = null;
        }
    }

    private void CheckGrounded()
    {
        if (timeSinceJump < groundedCooldown)
        {
            isGrounded = false;
            return;
        }

        float checkRadius = 0.3f;
        Vector3 checkPosition = transform.position + Vector3.down * 0.1f;
        isGrounded = Physics.CheckSphere(checkPosition, checkRadius, groundLayer);

        Debug.DrawRay(checkPosition, Vector3.down * 0.1f, isGrounded ? Color.green : Color.red);

        if (isGrounded)
        {
            canJumpAgain = true;
        }
    }
    public GameObject GetPulledObject()
    {
        return pulledObject;
    }


    public void StartPulling()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pullRange);
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Movable"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = hitCollider.gameObject;
                }
            }
        }

        if (closestObject != null)
        {
            pulledObject = closestObject;
            pulledObjectRb = pulledObject.GetComponent<Rigidbody>();
            isPulling = true;
        }
    }

    private void DetectNearbyFeedback()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        InteractionFeedback closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out InteractionFeedback feedback))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = feedback;
                }
            }
        }

        if (currentNearbyFeedback != null && currentNearbyFeedback != closest)
        {
            currentNearbyFeedback.HideFeedback();
        }

        if (closest != null && closest != currentNearbyFeedback)
        {
            closest.ShowFeedback();
        }

        currentNearbyFeedback = closest;
    }

    public bool IsHoldingInteract()
    {
        return isHoldingInteract;
    }
}
