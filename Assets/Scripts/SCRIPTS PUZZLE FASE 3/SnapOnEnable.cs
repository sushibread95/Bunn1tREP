using UnityEngine;

public class SnapOnEnable : MonoBehaviour
{
    public ConnectorSlot targetSlot;

    private void OnEnable()
    {
        ComponentConnector connector = GetComponent<ComponentConnector>();
        if (connector != null && targetSlot != null)
        {
            connector.SnapToSlot(targetSlot);
            Puzzle3Manager.Instance.CheckSolution();
        }
        else
        {
            Debug.LogWarning($"[SnapOnEnable] Falha ao snapar {gameObject.name}. Verifique se o slot está atribuído.");
        }
    }
}
