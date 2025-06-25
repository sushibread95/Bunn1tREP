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
    public CameraZoneManager cameraZoneManager; // referencia arrastada no Inspector





    private void Start()
    {
        activateButton.SetActive(false);
        deactivateButton.SetActive(false);
        cameraToActivate.Priority = inactivePriority;
    }

    public void ActivateCamera()
    {
        cameraToActivate.Priority = activePriority;

        if (cameraFollowsPlayer && cameraToActivate != null)
        {
            cameraToActivate.Follow = playerTransform;
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
            cameraZoneManager.SetCurrentCamera(cameraToActivate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activateButton.SetActive(false);
            deactivateButton.SetActive(false);
            cameraZoneManager.ClearCurrentCamera();
            DeactivateCamera();
        }
    }
}
