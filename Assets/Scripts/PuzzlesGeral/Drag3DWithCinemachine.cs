using UnityEngine;
using Unity.Cinemachine;

public class Drag3DWithCinemachine : MonoBehaviour
{
    [Header("Referência da câmera controlada pelo Cinemachine")]
    public Camera cinemachineCamera;

    [Header("Layer dos objetos arrastáveis (Ex: Movable)")]
    public LayerMask draggableLayer;

    private Transform draggedObject;
    private float dragDistance;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && draggedObject != null)
        {
            Drag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            EndDrag();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TryStartDrag(touchPos);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (draggedObject != null)
                        Drag(touchPos);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (draggedObject != null)
                        EndDrag();
                    break;
            }
        }
#endif
    }

    void TryStartDrag(Vector2 screenPosition)
    {
        Ray ray = cinemachineCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
        {
            draggedObject = hit.transform;
            dragDistance = Vector3.Distance(cinemachineCamera.transform.position, hit.point);
        }
    }

    void Drag(Vector2 screenPosition)
    {
        Ray ray = cinemachineCamera.ScreenPointToRay(screenPosition);
        Vector3 newPosition = ray.origin + ray.direction * dragDistance;
        draggedObject.position = newPosition;
    }

    void EndDrag()
    {
        draggedObject = null;
    }
}
