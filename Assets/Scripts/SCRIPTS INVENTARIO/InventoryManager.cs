using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots;
    public GameObject inventoryFullMessageUI;

    private void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>(true);
        if (inventoryFullMessageUI != null)
            inventoryFullMessageUI.SetActive(false);
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsOccupied)
            {
                // Instancia o item diretamente como filho do slot
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.transform.localPosition = Vector3.zero; // centraliza dentro do slot
                newItem.transform.localScale = Vector3.one;

                slot.SetItem(newItem);
                return true;
            }
        }

        Debug.Log("Oops! Inventário cheio.");
        if (inventoryFullMessageUI != null)
        {
            inventoryFullMessageUI.SetActive(true);
            Invoke(nameof(EsconderMensagem), 2f);
        }

        return false;
    }

    public bool TemEspacoDisponivel()
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsOccupied)
                return true;
        }

        return false;
    }

    private void EsconderMensagem()
    {
        if (inventoryFullMessageUI != null)
            inventoryFullMessageUI.SetActive(false);
    }
}
