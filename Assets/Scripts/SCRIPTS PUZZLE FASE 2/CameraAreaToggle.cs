using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class CameraAreaToggle : MonoBehaviour
{
    [Header("Câmera")]
    public CinemachineCamera cameraToActivate;
    public int activePriority = 20;
    public int inactivePriority = 0;
    public bool cameraFollowsPlayer = false;
    public Transform playerTransform;

    [Header("UI")]
    public GameObject activateButton;
    public GameObject deactivateButton;

    private void Start()
    {
        activateButton.SetActive(false);
        deactivateButton.SetActive(false);
        cameraToActivate.Priority = inactivePriority;
    }

    public void ActivateCamera()
    {
        cameraToActivate.Priority = activePriority;

        if (cameraFollowsPlayer && cameraToActivate is CinemachineVirtualCameraBase vCam)
        {
            vCam.Follow = playerTransform;
        }

        activateButton.SetActive(false);
        deactivateButton.SetActive(true);
    }

    public void DeactivateCamera()
    {
        cameraToActivate.Priority = inactivePriority;
        // Botões agora são controlados fora deste método
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activateButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Desativa botões manualmente antes de chamar a função
            activateButton.SetActive(false);
            deactivateButton.SetActive(false);

            DeactivateCamera();
        }
    }
}
