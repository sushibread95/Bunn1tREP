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
        if (slot == null) return;

        transform.position = slot.snapPoint.position;
        transform.rotation = slot.snapPoint.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        isConnected = true;
        currentSlot = slot;
    }

    public void Release()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        isConnected = false;
        currentSlot = null;
    }
}
