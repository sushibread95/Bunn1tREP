using UnityEngine;

public class SaveOnlyInScene : MonoBehaviour
{
    public void Save()
    {
        PlayerRoot playerRoot = Object.FindAnyObjectByType<PlayerRoot>();
        if (playerRoot != null)
        {
            SaveSystem.SaveGame(playerRoot.gameObject);
            Debug.Log("[SAVE] Jogo salvo na cena atual: " + playerRoot.gameObject.name);
        }
        else
        {
            Debug.LogError("[SAVE] PlayerRoot não encontrado na cena para salvar.");
        }
    }
}
