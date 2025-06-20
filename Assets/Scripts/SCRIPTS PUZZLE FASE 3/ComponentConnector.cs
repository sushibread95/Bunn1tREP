using UnityEngine;

public class ComponentConnector : MonoBehaviour
{
    public string componentID; // Ex: "C", "M", "Y", "K"
    [HideInInspector] public bool isConnected = false;
    [HideInInspector] public ConnectorSlot currentSlot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SnapToSlot(ConnectorSlot slot)
    {
        if (slot == null)
        {
            Debug.LogWarning("[ComponentConnector] Slot nulo. Não foi possível snapar.");
            return;
        }

        Transform snapReference = slot.snapPoint != null ? slot.snapPoint : slot.transform;

        transform.position = snapReference.position;
        transform.rotation = snapReference.rotation;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        isConnected = true;
        currentSlot = slot;

        Debug.Log($"[ComponentConnector] '{name}' snapado ao slot '{slot.name}'");
    }

    public void Release()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        isConnected = false;
        currentSlot = null;
    }
}
