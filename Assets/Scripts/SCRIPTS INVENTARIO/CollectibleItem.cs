using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public RectTransform InventoryPanel;
    public GameObject InventoryItemPrefab;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindAnyObjectByType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogWarning($"[CollectibleItem] InventoryManager não encontrado na cena para {gameObject.name}");
            }
        }
    }

    public void Coletar()
    {
        if (InventoryItemPrefab == null || InventoryPanel == null || inventoryManager == null)
        {
            Debug.LogWarning($"[CollectibleItem] InventoryItemPrefab, InventoryPanel ou InventoryManager não configurado em {gameObject.name}");
            return;
        }

        bool adicionado = inventoryManager.AddItem(InventoryItemPrefab);
        if (adicionado)
        {
            Debug.Log($"[CollectibleItem] {gameObject.name} foi adicionado ao inventário.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventário cheio. Item não coletado.");
        }
    }

    public void SetInventoryReferences(GameObject itemPrefab, RectTransform panel, InventoryManager manager)
    {
        InventoryItemPrefab = itemPrefab;
        InventoryPanel = panel;
        inventoryManager = manager;
    }
}
