using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalSceneUI : MonoBehaviour
{
    [Header("Referências")]
    public CanvasGroup canvasGroup;
    public Button botaoMenu;
    public float duracaoFade = 1.5f;
    public float delayBotao = 2f; // tempo antes de mostrar o botão

    private void Start()
    {
        canvasGroup.alpha = 0f;
        botaoMenu.gameObject.SetActive(false); // esconde o botão
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float tempo = 0f;
        while (tempo < duracaoFade)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, tempo / duracaoFade);
            tempo += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Aguarda o tempo de delay antes de mostrar o botão
        yield return new WaitForSeconds(delayBotao);

        botaoMenu.gameObject.SetActive(true);
        botaoMenu.interactable = true;
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu inicial");
    }
}
