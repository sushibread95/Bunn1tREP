using UnityEngine;

public class Puzzle3IndicatorFeedback : MonoBehaviour
{
    public string indicatorID; // "C", "M", "Y", "K"
    public Renderer indicatorRenderer;
    public Material defaultMaterial;
    public Material connectedMaterial;
    public Material allConnectedMaterial;

    private bool isCorrectlyConnected = false;

    private void Start()
    {
        if (indicatorRenderer != null && defaultMaterial != null)
            indicatorRenderer.material = defaultMaterial;
    }

    public void SetConnected(bool correct)
    {
        isCorrectlyConnected = correct;

        if (indicatorRenderer != null)
        {
            if (correct && connectedMaterial != null)
                indicatorRenderer.material = connectedMaterial;
            else if (defaultMaterial != null)
                indicatorRenderer.material = defaultMaterial;
        }
    }

    public void SetAllConnected()
    {
        if (indicatorRenderer != null && allConnectedMaterial != null)
            indicatorRenderer.material = allConnectedMaterial;
    }
}
