using UnityEngine;

public class InteractionFeedback : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private bool isHighlighted = false;

    [SerializeField] private Material highlightMaterial;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void ShowFeedback()
    {
        if (highlightMaterial == null)
        {
            Debug.LogWarning($"[InteractionFeedback] Highlight material não definido em {gameObject.name}");
            return;
        }

        if (isHighlighted) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] newMats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < newMats.Length; j++)
            {
                newMats[j] = highlightMaterial;
            }
            renderers[i].materials = newMats;
        }

        isHighlighted = true;

        // 🔍 DEBUG VISUAL: Linha até o player
        if (Camera.main != null)
        {
            Debug.DrawLine(transform.position, Camera.main.transform.position, Color.yellow, 0.5f);
        }

        Debug.Log($"[InteractionFeedback] {gameObject.name} destacado.");
    }

    public void HideFeedback()
    {
        if (!isHighlighted) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        isHighlighted = false;
    }
}
