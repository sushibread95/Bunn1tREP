using UnityEngine;
using Unity.Cinemachine;

public class MobileCameraController : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomMin = 30f;
    [SerializeField] private float zoomMax = 45f;
    [SerializeField] private float zoomSpeed = 0.05f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 0.2f;

    private CinemachinePositionComposer composer;
    private Transform cameraTarget;

    private float lastTouchX;

    private void Start()
    {
        if (cinemachineCamera != null)
        {
            composer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
            cameraTarget = cinemachineCamera.Follow;
        }
        else
        {
            Debug.LogError("CinemachineCamera não foi atribuído no inspector!");
        }
    }

    private void Update()
    {
        HandlePinchToZoom();
        HandleCameraRotation();
    }

    private void HandlePinchToZoom()
    {
        if (Input.touchCount == 2 && composer != null)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevDistance = Vector2.Distance(touch0Prev, touch1Prev);
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);
            float deltaDistance = currentDistance - prevDistance;

            float newDistance = composer.CameraDistance - deltaDistance * zoomSpeed;
            composer.CameraDistance = Mathf.Clamp(newDistance, zoomMin, zoomMax);
        }
    }

    private void HandleCameraRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                if (cameraTarget != null)
                {
                    cameraTarget.Rotate(0, deltaX * rotationSpeed, 0);
                }
            }
        }
    }
}
