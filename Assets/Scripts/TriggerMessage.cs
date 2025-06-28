using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    [Header("Mensagem a ser ativada")]
    public GameObject mensagem;

    [Header("Duração da mensagem (segundos)")]
    public float duracao = 3f;

    private void Start()
    {
        if (mensagem != null)
            mensagem.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mensagem != null)
        {
            StopAllCoroutines(); // caso entre de novo antes de terminar
            StartCoroutine(MostrarMensagemTemporariamente());
        }
    }

    private System.Collections.IEnumerator MostrarMensagemTemporariamente()
    {
        mensagem.SetActive(true);
        yield return new WaitForSeconds(duracao);
        mensagem.SetActive(false);
    }
}
