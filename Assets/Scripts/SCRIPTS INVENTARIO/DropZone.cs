using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Objeto que será ativado após o item ser solto")]
    public GameObject objetoParaAtivar;

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

    public void OnItemSolto()
    {
        if (foiAtivado) return;

        // Tenta desativar parede ou objeto com lógica extra
        if (objetoParaDesativar != null)
        {
            PuzzleWall wall = objetoParaDesativar.GetComponent<PuzzleWall>();
            if (wall != null)
            {
                wall.HandlePuzzleSolved();
            }
            else
            {
                objetoParaDesativar.SetActive(false);
            }
        }

        // Ativa o novo objeto
        if (objetoParaAtivar != null)
            objetoParaAtivar.SetActive(true);

        // Toca som (se tiver)
        if (somAoAtivar != null)
            somAoAtivar.Play();

        // Dispara animação (se tiver)
        if (animador != null && !string.IsNullOrEmpty(triggerDeAtivacao))
            animador.SetTrigger(triggerDeAtivacao);

        foiAtivado = true;

        Debug.Log("DropZone ativada com sucesso!");
    }
}