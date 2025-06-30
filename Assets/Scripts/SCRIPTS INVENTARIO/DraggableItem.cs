using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;

    [Header("Prefab no mundo que será instanciado")]
    public GameObject worldItemPrefab;

    [Header("Camada onde o item pode ser solto")]
    public LayerMask dropLayerMask;

    [Header("ID do item (deve combinar com o da DropZone)")]
    public string itemID;

    [Header("Câmera usada para o Raycast (opcional)")]
    public Camera raycastCamera;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject player;

    private CollectibleItem originalCollectibleData;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[DraggableItem] Canvas não encontrado no pai. Certifique-se de que o item está dentro de um Canvas.");
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (worldItemPrefab != null)
        {
            originalCollectibleData = worldItemPrefab.GetComponent<CollectibleItem>();
        }

        if (raycastCamera == null)
        {
            raycastCamera = Camera.main;
            if (raycastCamera == null)
            {
                Debug.LogWarning("[DraggableItem] Nenhuma câmera definida e Camera.main não encontrada.");
            }
        }
    }

    private void Start()
    {
        if (worldItemPrefab == null)
            Debug.LogWarning($"[DraggableItem] 'worldItemPrefab' não está definido em {gameObject.name}");
        if (string.IsNullOrEmpty(itemID))
            Debug.LogWarning($"[DraggableItem] 'itemID' está vazio em {gameObject.name}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;

        if (canvas == null)
        {
            Debug.LogWarning("[DraggableItem] Canvas não está definido.");
            return;
        }

        Transform dragLayer = canvas.transform.Find("DragLayer/Canvas");
        transform.SetParent(dragLayer != null ? dragLayer : canvas.transform, true);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = raycastCamera != null ? raycastCamera.ScreenPointToRay(eventData.position) : new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, dropLayerMask))
        {
            Debug.Log($"[Raycast] Acertou: {hit.collider.gameObject.name}");
            DropZone dropZone = hit.collider.GetComponent<DropZone>();
            if (dropZone != null && !string.IsNullOrEmpty(dropZone.acceptedItemID) && dropZone.acceptedItemID == itemID)
            {
                Debug.Log($"[DraggableItem] DropZone válida com ID correspondente: {dropZone.acceptedItemID}");
                dropZone.OnItemSolto(itemID);
                Destroy(gameObject);
                return;
            }
            else
            {
                Debug.LogWarning("[DraggableItem] DropZone encontrada, mas ID não bateu ou está vazia. Retornando ao inventário.");
                ReturnToSlot();
                return;
            }
        }
        else
        {
            Debug.LogWarning("[Raycast] Nada foi atingido. Retornando ao inventário.");
            ReturnToSlot();
            return;
        }
    }

    private void ApplyOriginalReferencesTo(GameObject clone)
    {
        var collectible = clone.GetComponent<CollectibleItem>();
        if (collectible != null && originalCollectibleData != null)
        {
            collectible.SetInventoryReferences(
                originalCollectibleData.InventoryItemPrefab,
                originalCollectibleData.InventoryPanel,
                FindAnyObjectByType<InventoryManager>()
            );
        }
    }

    private void ReturnToSlot()
    {
        transform.SetParent(parentAfterDrag, true);
        rectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;
    }
}
