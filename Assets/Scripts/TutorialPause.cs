using UnityEngine;

public class BotaoPauseInicial : MonoBehaviour
{
    public GameObject botao; // O botão da UI (deve estar no Canvas com Sorting Order alto)
    public PauseManager pauseManager; // Referência ao PauseManager

    private const string chavePrimeiraVez = "BotaoPauseInicialFoiMostrado";

    void Start()
    {
        // Verifica se é a primeira execução
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

    // Chamar este método no OnClick do botão
    public void AoClicarNoBotao()
    {
        // Pausar o jogo usando o PauseManager
        if (pauseManager != null)
            pauseManager.PausarOuRetomar();

        // Esconde o botão após o clique
        if (botao != null)
            botao.SetActive(false);

        // Salva que o botão já foi mostrado
        PlayerPrefs.SetInt(chavePrimeiraVez, 1);
        PlayerPrefs.Save();
    }
}
