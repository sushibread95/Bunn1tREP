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
        if (slot == null)
        {
            Debug.LogWarning("[CableConnector] Slot nulo. Não foi possível snapar.");
            return;
        }

        Transform snapReference = slot.snapPoint != null ? slot.snapPoint : slot.transform;

        transform.position = snapReference.position;
        transform.rotation = snapReference.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        isConnected = true;
        currentSlot = slot;

        Debug.Log($"[CableConnector] '{name}' snapado ao slot '{slot.name}'");

        CablePuzzleManager.Instance.CheckSolution();
    }

    public void Release()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        isConnected = false;
        currentSlot = null;
    }
}
