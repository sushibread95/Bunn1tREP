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
        Camera cam = raycastCamera != null ? raycastCamera : null;
        Ray ray = cam != null ? cam.ScreenPointToRay(eventData.position) : new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, dropLayerMask))
        {
            DropZone dropZone = hit.collider.GetComponent<DropZone>();
            if (dropZone != null && dropZone.acceptedItemID == itemID)
            {
                GameObject clone = Instantiate(worldItemPrefab, hit.point, Quaternion.identity);
                ApplyOriginalReferencesTo(clone);
                dropZone.OnItemSolto();
                Destroy(gameObject);
                return;
            }
        }

        // Se não acertou nada, dropa perto do jogador
        Vector3 rayStart = player != null ? player.transform.position + player.transform.forward * 0.3f + Vector3.up * 0.3f : transform.position + Vector3.up * 0.3f;
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit groundHit, 2f, ~0))
        {
            GameObject clone = Instantiate(worldItemPrefab, groundHit.point + Vector3.up * 0.1f, Quaternion.identity);
            ApplyOriginalReferencesTo(clone);

            Rigidbody rb = clone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = true;
                rb.isKinematic = false;
            }

            Debug.Log("Item dropado com sucesso no chão.");
        }
        else
        {
            Debug.LogWarning("Nenhum chão detectado. Retornando item ao inventário.");
            ReturnToSlot();
            return;
        }

        Destroy(gameObject);
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
