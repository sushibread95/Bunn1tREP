using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    void Awake()
    {
        if (GameState.ShouldLoad)
        {
            GameState.ShouldLoad = false;
            SaveSystem.LoadGame(gameObject);
            Debug.Log("[LOAD] LoadGame chamado diretamente pelo PlayerRoot (via Awake).");
        }
    }
}
