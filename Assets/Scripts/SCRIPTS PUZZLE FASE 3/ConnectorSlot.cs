using UnityEngine;

public class ConnectorSlot : MonoBehaviour
{
    public string expectedID; // Ex: "C", "M", "Y", "K"
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        ComponentConnector component = other.GetComponent<ComponentConnector>();
        if (component != null && !component.isConnected && component.componentID == expectedID)
        {
            component.SnapToSlot(this);
            Puzzle3Manager.Instance.CheckSolution();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ComponentConnector component = other.GetComponent<ComponentConnector>();
        if (component != null && component.currentSlot == this && !component.isConnected)
        {
            component.Release();
        }
    }
}
