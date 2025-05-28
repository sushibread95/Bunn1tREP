using UnityEngine;

public class ObjectInteractionRange : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Distância mínima para permitir interação especial.")]
    public float activationRange = 4f;

    [Tooltip("Permite que o objeto seja puxado mesmo fora do pullRange padrão.")]
    public bool allowDistancePull = false;

    private bool isPlayerNear = false;

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool withinRange = distance <= activationRange;

        // Apenas guarda o estado — se quiser usar depois
        isPlayerNear = withinRange;
    }
}
