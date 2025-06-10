using UnityEngine;

public class VisorOlhosController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Transform visorTransform;
    public Transform olhosTransform;
    public Transform corpoTransform;

    [Header("Offset Posicional")]
    public Vector3 offsetPosicional;

    [Header("Animações")]
    public Animator visorAnimator;
    public Animator olhosAnimator;
    public string[] idleAnimations;
    public float idleCheckTime = 2f;

    [Header("Configuração de rotação")]
    public float rotacaoSuave = 5f;

    private float idleTimer;
    private bool isFalling;

    private Transform cameraTransform;
    private Rigidbody playerRb;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        playerRb = player.GetComponent<Rigidbody>();
        idleTimer = idleCheckTime;

        if (corpoTransform == null)
            corpoTransform = this.transform;
    }

    void Update()
    {
        corpoTransform.position = player.position + offsetPosicional;

        Vector3 playerVelocity = playerRb.linearVelocity;
        bool isMoving = playerVelocity.magnitude > 0.1f;

        if (isMoving)
        {
            Vector3 direction = playerVelocity.normalized;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                corpoTransform.rotation = Quaternion.Slerp(corpoTransform.rotation, targetRotation, Time.deltaTime * rotacaoSuave);
            }

            idleTimer = idleCheckTime;
        }
        else
        {
            // Parado: olhar para a câmera
            Vector3 lookDirection = cameraTransform.position - corpoTransform.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                corpoTransform.rotation = Quaternion.Slerp(corpoTransform.rotation, targetRotation, Time.deltaTime * rotacaoSuave);
            }

            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f && idleAnimations.Length > 0)
            {
                string idleName = idleAnimations[Random.Range(0, idleAnimations.Length)];
                if (visorAnimator) visorAnimator.Play(idleName);
                if (olhosAnimator) olhosAnimator.Play(idleName);
                idleTimer = idleCheckTime + Random.Range(0.5f, 1.5f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && isFalling)
        {
            if (visorAnimator) visorAnimator.SetTrigger("Fall");
            if (olhosAnimator) olhosAnimator.SetTrigger("Fall");
            isFalling = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFalling = true;
        }
    }
}
