using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Objeto que será ativado após o item ser solto")]
    public GameObject objetoParaAtivar;

    [Header("Segundo objeto a ser ativado")]
    public GameObject objetoParaAtivar2;

    [Header("Objeto que será desativado (ex: zona escura)")]
    public GameObject objetoParaDesativar;

    [Header("Som opcional")]
    public AudioSource somAoAtivar;

    [Header("Animação opcional")]
    public Animator animador;
    public string triggerDeAtivacao;

    [Header("ID aceito (deve bater com o item solto)")]
    public string acceptedItemID;

    private bool foiAtivado = false;

    private void OnValidate()
    {
        if (somAoAtivar == null)
        {
            Debug.LogWarning($"[DropZone] Campo 'somAoAtivar' está vazio em {gameObject.name}. Se não for usar som, ignore. Caso contrário, atribua um AudioSource válido.");
        }
    }

    public void OnItemSolto(string itemID)
    {
        Debug.Log("[DropZone] OnItemSolto() chamado com ID: " + itemID);

        if (foiAtivado)
        {
            Debug.Log("[DropZone] Já foi ativado anteriormente. Ignorando.");
            return;
        }

        if (!string.IsNullOrEmpty(acceptedItemID) && itemID != acceptedItemID)
        {
            Debug.LogWarning("[DropZone] Item incorreto. Esperado: '" + acceptedItemID + "', recebido: '" + itemID + "'. Ignorando.");
            return;
        }

        if (objetoParaDesativar != null)
        {
            Debug.Log("[DropZone] Tentando desativar objeto: " + objetoParaDesativar.name);
            PuzzleWall wall = objetoParaDesativar.GetComponent<PuzzleWall>();
            if (wall != null)
            {
                Debug.Log("[DropZone] Componente PuzzleWall encontrado. Chamando HandlePuzzleSolved().");
                wall.HandlePuzzleSolved();
            }
            else
            {
                Debug.Log("[DropZone] Nenhum PuzzleWall encontrado. Desativando diretamente.");
                objetoParaDesativar.SetActive(false);
            }
        }
        else
        {
            Debug.Log("[DropZone] Nenhum objeto para desativar foi atribuído.");
        }

        if (objetoParaAtivar != null)
        {
            Debug.Log("[DropZone] Ativando objeto 1: " + objetoParaAtivar.name);
            objetoParaAtivar.SetActive(true);
        }
        else
        {
            Debug.Log("[DropZone] Objeto 1 não atribuído.");
        }

        if (objetoParaAtivar2 != null)
        {
            Debug.Log("[DropZone] Ativando objeto 2: " + objetoParaAtivar2.name);
            objetoParaAtivar2.SetActive(true);
        }
        else
        {
            Debug.Log("[DropZone] Objeto 2 não atribuído.");
        }

        if (somAoAtivar != null)
        {
            Debug.Log("[DropZone] Tocando som.");
            somAoAtivar.Play();
        }
        else
        {
            Debug.Log("[DropZone] Nenhum AudioSource atribuído. Som não será tocado.");
        }

        if (animador != null && !string.IsNullOrEmpty(triggerDeAtivacao))
        {
            Debug.Log("[DropZone] Disparando animação: " + triggerDeAtivacao);
            animador.SetTrigger(triggerDeAtivacao);
        }

        foiAtivado = true;

        Debug.Log("[DropZone] DropZone ativada com sucesso!");
    }
}
