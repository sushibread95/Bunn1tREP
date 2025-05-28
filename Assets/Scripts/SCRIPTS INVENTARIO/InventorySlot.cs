using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool IsOccupied => transform.childCount > 0;

    public void SetItem(GameObject item)
    {
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero;
    }

    public void ClearSlot()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
