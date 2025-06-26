using UnityEngine;

public class CableSnap : MonoBehaviour
{
    public PuzzleLuzManager puzzleManager;
    public string targetTag = "CableTarget";
    public float snapDistance = 0.5f;

    private bool connected = false;

    [Header("Limites de Movimento")]
    public Collider areaLimitCollider; // Atribuir no Inspector

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
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, bounds.min.z, bounds.max.z);

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
                // Faz snap ao ponto de conexão (posição + rotação)
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;

                connected = true;

                // Desativa movimentação
                if (TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                }

                // Notifica o Puzzle Manager
                if (puzzleManager != null)
                {
                    Debug.Log("[Cable] Cabo conectado com sucesso.");
                    puzzleManager.CableConnected();
                }
                else
                {
                    Debug.LogWarning("[Cable] PuzzleManager não atribuído.");
                }
            }
        }
    }

}
