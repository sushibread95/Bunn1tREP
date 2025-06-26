using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    [Header("UI de Pausa")]
    public GameObject pauseMenuUI;
    public string nomeCenaMenu; // Ex: "MenuInicial"

    [Header("Objetos para esconder no pause (se estiverem ativos)")]
    public GameObject[] objetosParaOcultar;

    private bool jogoPausado = false;

    // Armazena quais estavam ativos antes do pause
    private Dictionary<GameObject, bool> estadoOriginal = new Dictionary<GameObject, bool>();

    void Start()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        estadoOriginal.Clear();
    }

    public void PausarOuRetomar()
    {
        jogoPausado = !jogoPausado;

        if (jogoPausado)
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            OcultarObjetos();
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            RestaurarObjetos();
        }
    }

    public void Retomar()
    {
        jogoPausado = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        RestaurarObjetos();
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

    private void OcultarObjetos()
    {
        estadoOriginal.Clear();
        foreach (GameObject obj in objetosParaOcultar)
        {
            if (obj != null)
            {
                estadoOriginal[obj] = obj.activeSelf;
                if (obj.activeSelf)
                    obj.SetActive(false);
            }
        }
    }

    private void RestaurarObjetos()
    {
        foreach (var kvp in estadoOriginal)
        {
            if (kvp.Key != null)
            {
                kvp.Key.SetActive(kvp.Value);
            }
        }
        estadoOriginal.Clear();
    }
}
