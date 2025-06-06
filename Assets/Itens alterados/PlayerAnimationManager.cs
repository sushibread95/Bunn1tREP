using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Referências ao Player (só leitura)")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Animators (visor, óculos, corpo etc.)")]
    [SerializeField] private Animator visorAnimator;
    [SerializeField] private Animator oculosAnimator;
    [SerializeField] private Animator corpoAnimator;

    [Header("Transform dos objetos visuais")]
    [SerializeField] private Transform visorTransform;
    [SerializeField] private Transform oculosTransform;
    [SerializeField] private Vector3 visorOffset;
    [SerializeField] private Vector3 oculosOffset;
    [SerializeField] private bool olharParaCamera = true;

    [Header("Idle")]
    [SerializeField] private float idleDelay = 5f;
    [SerializeField] private int idleVariacoes = 2;
    private float tempoParado = 0f;
    private Vector3 ultimaPosicao;
    private float movimentoMinimo = 0.05f;

    [Header("Detecção de Chão")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastAltura = 0.1f;
    [SerializeField] private float raycastDistancia = 0.3f;
    private bool estaNoChao = true;

    private void Start()
    {
        if (playerRigidbody == null && playerTransform != null)
            playerRigidbody = playerTransform.GetComponent<Rigidbody>();

        ultimaPosicao = playerTransform.position;
        StartCoroutine(RotinaIdleAleatorio());
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        AtualizarPosicaoVisual();
        VerificarMovimento();
        VerificarQueda();
    }

    private void AtualizarPosicaoVisual()
    {
        if (visorTransform != null)
        {
            visorTransform.position = playerTransform.position + visorOffset;
            visorTransform.rotation = olharParaCamera && Camera.main ?
                Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) :
                Quaternion.identity;
        }

        if (oculosTransform != null)
        {
            oculosTransform.position = playerTransform.position + oculosOffset;
            oculosTransform.rotation = olharParaCamera && Camera.main ?
                Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up) :
                Quaternion.identity;
        }
    }

    private void VerificarMovimento()
    {
        float deslocamento = (playerTransform.position - ultimaPosicao).magnitude;

        tempoParado = deslocamento < movimentoMinimo
            ? tempoParado + Time.deltaTime
            : 0f;

        ultimaPosicao = playerTransform.position;
    }

    private IEnumerator RotinaIdleAleatorio()
    {
        while (true)
        {
            if (tempoParado > idleDelay && estaNoChao)
            {
                int index = Random.Range(0, idleVariacoes);
                visorAnimator?.SetInteger("IdleIndex", index);
                oculosAnimator?.SetInteger("IdleIndex", index);
                corpoAnimator?.SetInteger("IdleIndex", index);

                tempoParado = 0f;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void VerificarQueda()
    {
        Vector3 origem = playerTransform.position + Vector3.up * raycastAltura;
        bool tocandoChao = Physics.Raycast(origem, Vector3.down, raycastDistancia, groundLayer);

        if (tocandoChao && !estaNoChao)
        {
            estaNoChao = true;
            PlayTrigger("Caindo");
        }
        else if (!tocandoChao && estaNoChao)
        {
            estaNoChao = false;
        }
    }

    private void PlayTrigger(string nome)
    {
        if (string.IsNullOrEmpty(nome)) return;

        visorAnimator?.ResetTrigger(nome);
        oculosAnimator?.ResetTrigger(nome);
        corpoAnimator?.ResetTrigger(nome);

        visorAnimator?.SetTrigger(nome);
        oculosAnimator?.SetTrigger(nome);
        corpoAnimator?.SetTrigger(nome);
    }
}
