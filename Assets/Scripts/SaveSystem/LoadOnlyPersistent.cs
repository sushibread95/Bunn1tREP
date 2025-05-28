using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnlyPersistent : MonoBehaviour
{
    private void Awake()
    {
        // Impede duplicação
        if (FindObjectsByType<LoadOnlyPersistent>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Debug.Log("[LOAD MANAGER] Persistente entre cenas.");
    }

    public void Load()
    {
        Debug.Log("[LOAD] Botão Load foi clicado.");
        StartCoroutine(LoadSceneAndApplyData());
    }

    private IEnumerator LoadSceneAndApplyData()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Garagem_FINAL");
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;
        Debug.Log("[LOAD] Cena ativada. Aguardando próximo frame para iniciar busca por PlayerRoot...");
        yield return null;

        yield return StartCoroutine(FindAndLoadPlayerRoot());
    }

    private IEnumerator FindAndLoadPlayerRoot()
    {
        Debug.Log("[LOAD] Entrando na busca por PlayerRoot após o frame.");

        PlayerRoot root = null;
        float timeout = 10f;
        float elapsed = 0f;

        while (elapsed < timeout)
        {
            try
            {
                var all = UnityEngine.Object.FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
                Debug.Log($"[DEBUG] Quantos PlayerRoot encontrados? {all.Length}");

                foreach (var obj in all)
                {
                    Debug.Log($"[DEBUG] Encontrado: {obj.name} | Ativo: {obj.gameObject.activeInHierarchy}");
                }

                root = all.Length > 0 ? all[0] : null;

                if (root != null && root.gameObject.activeInHierarchy)
                    break;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[LOAD] Erro durante busca: " + ex.Message);
                yield break;
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (root != null)
        {
            SaveSystem.LoadGame(root.gameObject);
            Debug.Log("[LOAD] Dados aplicados no objeto: " + root.gameObject.name);
        }
        else
        {
            Debug.LogError("[LOAD] PlayerRoot não encontrado após timeout.");
        }
    }
}
