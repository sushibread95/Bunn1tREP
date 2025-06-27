using UnityEngine;

public class CableSnap : MonoBehaviour
{
    public PuzzleLuzManager puzzleManager;
    public string targetTag = "CableTarget";
    public float snapDistance = 0.5f;

    private bool connected = false;
    private float initialZ;

    [Header("Limites de Movimento")]
    public Collider areaLimitCollider;

    [Header("Objeto com cabo conectado")]
    public GameObject connectedCableObject;

    [Header("Objeto a ser desativado após conexão")]
    public GameObject objectToDisable;

    private void Start()
    {
        // Armazena o valor original do eixo Z
        initialZ = transform.position.z;
    }

    private void Update()
    {
        if (areaLimitCollider != null && !connected)
        {
            KeepInsideBounds();
        }
    }

    private void KeepInsideBounds()
    {
        Bounds bounds = areaLimitCollider.bounds;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bounds.min.x, bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bounds.min.y, bounds.max.y);
        clampedPosition.z = initialZ; // Z travado

        transform.position = clampedPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (connected) return;

        if (other.CompareTag(targetTag))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= snapDistance)
            {
                // Snap na posição e rotação
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;

                connected = true;

                if (TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                }

                if (puzzleManager != null)
                {
                    Debug.Log("[Cable] Cabo conectado com sucesso.");
                    puzzleManager.CableConnected();
                }
                else
                {
                    Debug.LogWarning("[Cable] PuzzleManager não atribuído.");
                }

                // Ativa o objeto conectado
                if (connectedCableObject != null)
                {
                    connectedCableObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("[Cable] connectedCableObject não atribuído.");
                }

                // Desativa o objeto especificado
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("[Cable] objectToDisable não atribuído.");
                }
            }
        }
    }
}
