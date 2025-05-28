using UnityEngine;

public class CableMonitorFeedback : MonoBehaviour
{
    public string monitorID; // "R", "G", "B"
    public Renderer monitorRenderer;
    public Material defaultMaterial;
    public Material connectedMaterial;
    public Material allConnectedMaterial;

    private bool isCorrectlyConnected = false;

    private void Start()
    {
        if (monitorRenderer != null && defaultMaterial != null)
            monitorRenderer.material = defaultMaterial;
    }

    public void SetConnected(bool correct)
    {
        isCorrectlyConnected = correct;

        if (monitorRenderer != null)
        {
            if (correct && connectedMaterial != null)
                monitorRenderer.material = connectedMaterial;
            else if (defaultMaterial != null)
                monitorRenderer.material = defaultMaterial;
        }
    }

    public void SetAllConnected()
    {
        if (monitorRenderer != null && allConnectedMaterial != null)
            monitorRenderer.material = allConnectedMaterial;
    }
}
