using UnityEngine;

public class GlassesFollow : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform playerHead;             // Cabe�a do jogador
    public Transform cameraTransform;        // C�mera (Camera.main.transform)
    public Animator glassesAnimator;         // Animator do �culos

    [Header("Offset (opcional)")]
    public Vector3 positionOffset = new Vector3(0f, 0.1f, 0.05f);

    void LateUpdate()
    {
        if (playerHead == null || cameraTransform == null)
            return;

        // Posiciona com offset rotacionado
        transform.position = playerHead.position + playerHead.rotation * positionOffset;

        // Faz os �culos olharem para frente da c�mera
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
    }

    void Update()
    {
        // Exemplo: ativa anima��o ao apertar tecla T
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
            Debug.LogWarning("Animator n�o est� atribu�do no script dos �culos.");
        }
    }
}
