using System.Collections;
using UnityEngine;

public class PauseSaveHandler : MonoBehaviour
{
    public void SalvarComFísica()
    {
        StartCoroutine(SaveAfterFixedUpdate());
    }

    private IEnumerator SaveAfterFixedUpdate()
    {
        Debug.Log("[SAVE] Iniciando salvamento com física garantida...");
        Time.timeScale = 1f;
        yield return new WaitForFixedUpdate();

        PlayerRoot root = Object.FindAnyObjectByType<PlayerRoot>();
        if (root != null)
        {
            SaveSystem.SaveGame(root.gameObject);
            Debug.Log("[SAVE] Salvo com sucesso após física: " + root.transform.position);
        }
        else
        {
            Debug.LogWarning("[SAVE] PlayerRoot não encontrado.");
        }

        Time.timeScale = 0f;
    }
}
