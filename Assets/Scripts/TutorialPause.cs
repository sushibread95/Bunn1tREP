using UnityEngine;

public class BotaoPauseInicial : MonoBehaviour
{
    public GameObject botao; // O bot�o da UI (deve estar no Canvas com Sorting Order alto)
    public PauseManager pauseManager; // Refer�ncia ao PauseManager

    private const string chavePrimeiraVez = "BotaoPauseInicialFoiMostrado";

    void Start()
    {
        // Verifica se � a primeira execu��o
        if (!PlayerPrefs.HasKey(chavePrimeiraVez))
        {
            if (botao != null)
                botao.SetActive(true);
        }
        else
        {
            if (botao != null)
                botao.SetActive(false);
        }
    }

    // Chamar este m�todo no OnClick do bot�o
    public void AoClicarNoBotao()
    {
        // Pausar o jogo usando o PauseManager
        if (pauseManager != null)
            pauseManager.PausarOuRetomar();

        // Esconde o bot�o ap�s o clique
        if (botao != null)
            botao.SetActive(false);

        // Salva que o bot�o j� foi mostrado
        PlayerPrefs.SetInt(chavePrimeiraVez, 1);
        PlayerPrefs.Save();
    }
}
