using UnityEngine;

public class DistanceInteractionFeedback : MonoBehaviour
{
    [Header("Highlight por Distância")]
    public float activationRange = 4f;
    public float requiredHeightDifference = 0.5f;
    public bool highlightOnlyByDistance = true;
    [SerializeField] private Material highlightMaterial;

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private bool isHighlighted = false;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    void Update()
    {
        if (!highlightOnlyByDistance) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        float heightDifference = Mathf.Abs(transform.position.y - player.transform.position.y);

        if (distance <= activationRange && heightDifference >= requiredHeightDifference && !isHighlighted)
        {
            ApplyHighlight();
        }
        else if ((distance > activationRange || heightDifference < requiredHeightDifference) && isHighlighted)
        {
            RemoveHighlight();
        }
    }

    private void ApplyHighlight()
    {
        if (highlightMaterial == null) return;

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
    }

    private void RemoveHighlight()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        isHighlighted = false;
    }

    public void ShowFeedback()
    {
        ApplyHighlight();
    }

    public void HideFeedback()
    {
        RemoveHighlight();
    }
}
