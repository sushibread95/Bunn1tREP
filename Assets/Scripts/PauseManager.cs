using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string nomeCenaMenu; // Ex: "MenuInicial"

    private bool jogoPausado = false;

    void Start()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void PausarOuRetomar()
    {
        jogoPausado = !jogoPausado;

        if (jogoPausado)
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
        }
    }

    public void Retomar()
    {
        jogoPausado = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void ReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }
}
