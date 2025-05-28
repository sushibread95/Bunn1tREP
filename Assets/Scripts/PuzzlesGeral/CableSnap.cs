using UnityEngine;

public class CableSnap : MonoBehaviour
{
    public PuzzleLuzManager puzzleManager; // Atribua via Inspector
    public string targetTag = "CableTarget"; // Tag dos pontos de conexão
    public float snapDistance = 0.5f; // Distância para snap
    private bool connected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (connected) return;

        if (other.CompareTag(targetTag))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance <= snapDistance)
            {
                // Faz snap ao ponto
                transform.position = other.transform.position;
                connected = true;

                // Opcional: desativa movimentação
                if (TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                }

                // Chama o Puzzle Manager
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
