using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class CameraZoneManager : MonoBehaviour
{
    public Button activateButton;
    public Button deactivateButton;

    private CinemachineCamera currentCamera;
    public int activePriority = 20;
    public int inactivePriority = 0;

    private void Start()
    {
        activateButton.gameObject.SetActive(false);
        deactivateButton.gameObject.SetActive(false);

        activateButton.onClick.AddListener(ActivateCamera);
        deactivateButton.onClick.AddListener(DeactivateCamera);
    }

    public void SetCurrentCamera(CinemachineCamera cam)
    {
        currentCamera = cam;
        activateButton.gameObject.SetActive(true);
        deactivateButton.gameObject.SetActive(false);
    }

    public void ClearCurrentCamera()
    {
        DeactivateCamera(); // Desativa ao sair
        currentCamera = null;
        activateButton.gameObject.SetActive(false);
        deactivateButton.gameObject.SetActive(false);
    }

    public void ActivateCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = activePriority;
            activateButton.gameObject.SetActive(false);
            deactivateButton.gameObject.SetActive(true);
        }
    }

    public void DeactivateCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = inactivePriority;
            activateButton.gameObject.SetActive(true);
            deactivateButton.gameObject.SetActive(false);

            // Desativa lógica do puzzle se existir
            
        }
    }
}
