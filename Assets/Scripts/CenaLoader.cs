using UnityEngine;
using UnityEngine.SceneManagement;

public class CenaLoader : MonoBehaviour
{
    public float delay = 0f; // Tempo de espera antes de carregar a cena

    public void CarregarCena(string nomeDaCena)
    {
        StartCoroutine(EsperarECarregar(nomeDaCena));
    }

    private System.Collections.IEnumerator EsperarECarregar(string nomeDaCena)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nomeDaCena);
    }
}
