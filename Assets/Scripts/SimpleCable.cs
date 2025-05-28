using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableRenderer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    [Range(0.1f, 10f)]
    public float cableLength = 2f; // Comprimento máximo do cabo
    public int segments = 20;
    public float sagIntensity = 0.5f;

    private LineRenderer lineRenderer;
    private Vector3[] points;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments;
        points = new Vector3[segments];

        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;
    }

    void Update()
    {
        DrawCable();
    }

    void DrawCable()
    {
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        float distance = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;

        // Corrige se a distância for maior que o comprimento do cabo
        float stretchFactor = Mathf.Min(1f, distance / cableLength);
        Vector3 effectiveEnd = start + direction * (cableLength * stretchFactor);

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            // Lerp linear
            Vector3 point = Vector3.Lerp(start, effectiveEnd, t);

            // Sag com base em uma curva senoidal
            float sag = Mathf.Sin(t * Mathf.PI) * sagIntensity * (1f - stretchFactor);
            point += Vector3.down * sag;

            points[i] = point;
        }

        lineRenderer.SetPositions(points);
    }
}
