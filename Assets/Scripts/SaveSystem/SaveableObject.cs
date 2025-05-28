using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    [SerializeField] private string uniqueId; // ID único atribuído manualmente no Inspector
    public string GetUniqueId() => uniqueId;
}