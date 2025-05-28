using UnityEngine;

public class GlassesFollow : MonoBehaviour
{
    [Header("Referências")]
    public Transform playerHead;             // Cabeça do jogador
    public Transform cameraTransform;        // Câmera (Camera.main.transform)
    public Animator glassesAnimator;         // Animator do óculos

    [Header("Offset (opcional)")]
    public Vector3 positionOffset = new Vector3(0f, 0.1f, 0.05f);

    void LateUpdate()
    {
        if (playerHead == null || cameraTransform == null)
            return;

        // Posiciona com offset rotacionado
        transform.position = playerHead.position + playerHead.rotation * positionOffset;

        // Faz os óculos olharem para frente da câmera
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
    }

    void Update()
    {
        // Exemplo: ativa animação ao apertar tecla T
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayAnimation("TocarBotao");
        }
    }

    public void PlayAnimation(string triggerName)
    {
        if (glassesAnimator != null)
        {
            glassesAnimator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("Animator não está atribuído no script dos óculos.");
        }
    }
}
