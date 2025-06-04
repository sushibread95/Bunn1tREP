using UnityEngine;

public class Cenoura : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(1.5f, 1.0f, 0f);
    public float followSpeed = 3.0f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1.0f;

    [Header("Partes destacáveis")]
    public GameObject parte1;
    public GameObject parte2;
    public Animator parte1Animator;
    public Animator parte2Animator;

    [Header("VFX")]
    public GameObject vfxPrefab;
    private GameObject activeVFX;

    private Vector3 targetPosition;
    private float floatTimer;
    private bool isBeingPulled = false;
    private bool hasStartedPullAnimation = false;

    void Update()
    {
        // Movimento flutuante enquanto não está sendo puxado
        if (!isBeingPulled)
        {
            floatTimer += Time.deltaTime * floatFrequency;
            float floatOffset = Mathf.Sin(floatTimer) * floatAmplitude;
            targetPosition = player.position + offset + new Vector3(0f, floatOffset, 0f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        // VFX centralizado entre partes
        if (isBeingPulled && activeVFX == null && vfxPrefab != null)
        {
            Vector3 midpoint = (parte1.transform.position + parte2.transform.position) / 2f;
            activeVFX = Instantiate(vfxPrefab, midpoint, Quaternion.identity, transform);
        }
        else if (!isBeingPulled && activeVFX != null)
        {
            Destroy(activeVFX);
        }

        // Manter VFX centralizado
        if (activeVFX != null)
        {
            Vector3 midpoint = (parte1.transform.position + parte2.transform.position) / 2f;
            activeVFX.transform.position = midpoint;
        }
    }

    public void StartBeingPulled()
    {
        Debug.Log("[Cenoura] StartBeingPulled chamado");

        if (isBeingPulled) return;

        isBeingPulled = true;

        if (parte1Animator != null)
        {
            parte1Animator.SetTrigger("StartSplit");
            Debug.Log("[Cenoura] Trigger StartSplit enviada para parte1Animator");
        }

        if (parte2Animator != null)
        {
            parte2Animator.SetTrigger("StartSplit");
            Debug.Log("[Cenoura] Trigger StartSplit enviada para parte2Animator");
        }
    }


    public void StopBeingPulled()
    {
        if (!isBeingPulled) return;

        isBeingPulled = false;

        // Etapa 3: animação reversa
        if (parte1Animator != null) parte1Animator.SetTrigger("EndSplit");
        if (parte2Animator != null) parte2Animator.SetTrigger("EndSplit");

        hasStartedPullAnimation = false;
    }
}
