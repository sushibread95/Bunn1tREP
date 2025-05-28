using UnityEngine;
using TMPro;

public class MensagemTrigger : MonoBehaviour
{
    [SerializeField] private GameObject mensagemUI;    // Painel com texto
    [SerializeField] private float tempoNaTela = 3f;   // Quanto tempo a mensagem fica visível
    [SerializeField] private float delayAntesDeMostrar = 1f; // Atraso antes da mensagem aparecer

    private bool jaMostrou = false;

    public void MostrarMensagem()
    {
        if (jaMostrou) return;

        jaMostrou = true;
        Invoke(nameof(ExibirMensagem), delayAntesDeMostrar);
    }

    private void ExibirMensagem()
    {
        if (mensagemUI != null)
        {
            mensagemUI.SetActive(true);
            Invoke(nameof(EsconderMensagem), tempoNaTela);
        }
    }

    private void EsconderMensagem()
    {
        if (mensagemUI != null)
            mensagemUI.SetActive(false);
    }
}
