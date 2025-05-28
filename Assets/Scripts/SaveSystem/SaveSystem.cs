using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public static class SaveSystem
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

    public static void SaveGame(GameObject player)
    {
        if (!player.TryGetComponent<PlayerRoot>(out _))
        {
            Debug.LogError("[SAVE] Objeto não é um jogador válido (PlayerRoot ausente)!");
            return;
        }

        SaveData data = new SaveData
        {
            playerPosition = new Vector3Data(player.transform.position),
            playerRotation = new Vector3Data(player.transform.eulerAngles)
        };
        Debug.Log($"[SAVE] Posição salva: {data.playerPosition.ToVector3()}");
        Debug.Log($"[SAVE] Rotação salva: {data.playerRotation.ToVector3()}");

        SaveableObject[] saveables = UnityEngine.Object.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        data.objectsData = new List<SaveableObjectData>();

        foreach (SaveableObject saveable in saveables)
        {
            GameObject obj = saveable.gameObject;
            SaveableObjectData objData = new SaveableObjectData
            {
                uniqueId = saveable.GetUniqueId(),
                position = new Vector3Data(obj.transform.position),
                rotation = new Vector3Data(obj.transform.eulerAngles),
                isActive = obj.activeSelf
            };
            data.objectsData.Add(objData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[SAVE] Jogo salvo em: " + savePath);
    }

    public static void LoadGame(GameObject player)
    {
        if (!player.TryGetComponent<PlayerRoot>(out _))
        {
            Debug.LogError("[LOAD] Objeto inválido para carregar dados (PlayerRoot ausente)!");
            return;
        }

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("[LOAD] Arquivo de save não encontrado!");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        player.transform.position = data.playerPosition.ToVector3();
        player.transform.eulerAngles = data.playerRotation.ToVector3();

        Debug.Log($"[LOAD] Posição carregada: {player.transform.position}");
        Debug.Log($"[LOAD] Rotação carregada: {player.transform.eulerAngles}");

        SaveableObject[] allObjects = UnityEngine.Object.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        foreach (SaveableObjectData objData in data.objectsData)
        {
            foreach (SaveableObject obj in allObjects)
            {
                if (obj.GetUniqueId() == objData.uniqueId)
                {
                    obj.transform.position = objData.position.ToVector3();
                    obj.transform.eulerAngles = objData.rotation.ToVector3();
                    obj.gameObject.SetActive(objData.isActive);
                    break;
                }
            }
        }

        Debug.Log("[LOAD] Dados aplicados com sucesso.");
    }
}
