using UnityEngine;

public class CableConnector : MonoBehaviour
{
    public string cableID; // Ex: "R", "G", "B"
    [HideInInspector] public bool isConnected = false;
    [HideInInspector] public CableSlot currentSlot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SnapToSlot(CableSlot slot)
    {
        if (slot == null) return;

        // Posiciona exatamente no SnapPoint
        transform.position = slot.snapPoint.position;
        transform.rotation = slot.snapPoint.rotation;

        // Zera movimentos físicos
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Congela tudo para evitar rodopios
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Atualiza estado
        isConnected = true;
        currentSlot = slot;

        // Verifica se todos os cabos estão no lugar após o encaixe
        CablePuzzleManager.Instance.CheckSolution();
    }

    public void Release()
    {
        // Libera movimento novamente
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        isConnected = false;
        currentSlot = null;
    }
}
