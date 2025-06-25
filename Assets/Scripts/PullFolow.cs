using UnityEngine;

public class PullFollower : MonoBehaviour
{
    public PlayerController playerController;
    public Transform player;
    public Cenoura cenoura;

    [Header("Visual & Efeitos")]
    public AudioSource pullingSound;
    public ParticleSystem pullingVFX;

    [Tooltip("Velocidade da rotação no eixo Y enquanto puxa")]
    public float spinSpeed = 180f;
    public int spinDirection = 1;

    [Header("Espelhamento e Offset")]
    public bool invertX = false;
    public float baseRotationX = 90f; // 90 ou -90
    public Vector3 positionOffset = Vector3.zero;

    [Header("Movimento")]
    public float pullFollowSpeed = 5f;
    public float returnSpeed = 3f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private float floatTimer;
    private bool isPullingActive = false;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    void Start()
    {
        if (player == null || playerController == null || cenoura == null)
        {
            Debug.LogError("PullFollower: Player, PlayerController ou Cenoura não definidos!");
            enabled = false;
            return;
        }

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        floatTimer += Time.deltaTime * floatFrequency;
        float floatOffset = Mathf.Sin(floatTimer) * floatAmplitude;

        GameObject pulledObject = playerController.GetPulledObject();

        if (playerController.IsHoldingInteract() && pulledObject != null)
        {
            if (!isPullingActive)
            {
                isPullingActive = true;
                cenoura.enabled = false;

                if (pullingSound != null && !pullingSound.isPlaying)
                    pullingSound.Play();

                if (pullingVFX != null && !pullingVFX.isPlaying)
                    pullingVFX.Play();
            }

            // Define posição com offset flutuante
            Vector3 pullStart = player.position;
            Vector3 pullEnd = pulledObject.transform.position;
            Vector3 midPoint = (pullStart + pullEnd) / 2f;

            Vector3 offsetWorld = transform.right * positionOffset.x +
                                  transform.up * positionOffset.y +
                                  transform.forward * positionOffset.z;

            Vector3 targetPosition = midPoint + offsetWorld + new Vector3(0, floatOffset, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, pullFollowSpeed * Time.deltaTime);

            // Direção para olhar no plano XZ
            Vector3 directionToTarget = pulledObject.transform.position - transform.position;
            directionToTarget.y = 0f;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                Quaternion upright = Quaternion.Euler(baseRotationX, lookRotation.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, upright, pullFollowSpeed * Time.deltaTime);
            }

            // Gira no eixo Y (local)
            transform.Rotate(Vector3.up, spinSpeed * spinDirection * Time.deltaTime, Space.Self);

            // Espelhar escala X, se necessário
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x) * (invertX ? -1 : 1);
            transform.localScale = currentScale;
        }
        else
        {
            if (isPullingActive)
            {
                isPullingActive = false;
                cenoura.enabled = true;

                if (pullingSound != null && pullingSound.isPlaying)
                    pullingSound.Stop();

                if (pullingVFX != null && pullingVFX.isPlaying)
                    pullingVFX.Stop();
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, returnSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalLocalRotation, returnSpeed * Time.deltaTime);
        }
    }
}
