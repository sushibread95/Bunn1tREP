using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileCameraController : MonoBehaviour
{
    public Transform target;
    public Vector2 rotationSpeed = new Vector2(2f, 2f);
    public Vector2 yClamp = new Vector2(-30f, 60f);
    public float distance = 5f;

    [Header("Referência da Área de Toque")]
    public RectTransform touchAreaUI;

    private float yaw = 0f;
    private float pitch = 20f;
    private int touchFingerId = -1;

    void LateUpdate()
    {
        if (Input.touchCount > 0 && touchAreaUI != null)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began &&
                    IsTouchInsideUIRect(touch.position) &&
                    !IsTouchOverUI(touch))
                {
                    touchFingerId = touch.fingerId;
                }

                if (touch.fingerId == touchFingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        yaw += touch.deltaPosition.x * rotationSpeed.x * Time.deltaTime;
                        pitch -= touch.deltaPosition.y * rotationSpeed.y * Time.deltaTime;
                        pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        touchFingerId = -1;
                    }
                }
            }
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance);

        transform.position = targetPosition;
        transform.rotation = rotation;
    }

    bool IsTouchOverUI(Touch touch)
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    bool IsTouchInsideUIRect(Vector2 screenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(touchAreaUI, screenPosition, null);
    }
}
