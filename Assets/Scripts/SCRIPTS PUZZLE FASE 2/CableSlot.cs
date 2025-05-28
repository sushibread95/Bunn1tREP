using UnityEngine;

public class CableSlot : MonoBehaviour
{
    public string expectedID; // Ex: "R", "G", "B"
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        CableConnector cable = other.GetComponent<CableConnector>();
        if (cable != null && !cable.isConnected && cable.cableID == expectedID)
        {
            cable.SnapToSlot(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CableConnector cable = other.GetComponent<CableConnector>();
        if (cable != null && cable.currentSlot == this && !cable.isConnected)
        {
            cable.Release();
        }
    }
}
