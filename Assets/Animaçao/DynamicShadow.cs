using UnityEngine;

public class DynamicShadow : MonoBehaviour
{
    public Transform target; // personagem
    public float maxScale = 1.0f;
    public float minScale = 0.3f;
    public float maxDistance = 5.0f;
    public LayerMask groundLayer;

    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(target.position, Vector3.down, out hit, maxDistance, groundLayer))
        {
            // Posiciona a sombra no chão
            transform.position = hit.point + Vector3.up * 0.01f;

            // Calcula a escala com base na altura
            float distance = Vector3.Distance(target.position, hit.point);
            float t = Mathf.InverseLerp(0f, maxDistance, distance);
            float scale = Mathf.Lerp(maxScale, minScale, t);
            transform.localScale = new Vector3(scale, 1f, scale);

           
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            // Se não tiver chão abaixo, opcionalmente desativa sombra
            transform.localScale = Vector3.zero;
        }
    }
}
