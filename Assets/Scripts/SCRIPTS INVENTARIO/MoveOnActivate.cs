using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveOnActivateWithPhysics : MonoBehaviour
{
    [Header("Distância que o objeto deve percorrer")]
    public float moveDistance = 5f;

    [Header("Velocidade de movimento")]
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool shouldMove = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Trava gravidade e movimentos em X e Y
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotation;

        startPosition = transform.position;
        targetPosition = startPosition + transform.forward * moveDistance;
        shouldMove = true;
    }

    void FixedUpdate()
    {
        if (!shouldMove) return;

        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
        {
            shouldMove = false;

            // Opcional: destrava a física após o movimento
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
