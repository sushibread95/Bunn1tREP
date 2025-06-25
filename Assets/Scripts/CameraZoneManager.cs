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
        Debug.Log("Botão foi clicado");

        if (currentCamera != null)
        {
            Debug.Log("Câmera encontrada: ativando.");
            currentCamera.Priority = activePriority;
            activateButton.gameObject.SetActive(false);
            deactivateButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Nenhuma câmera ativa atribuída!");
        }
    }


    public void DeactivateCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = inactivePriority;

            var mobileControl = currentCamera.GetComponent<MobileCameraController>();
            if (mobileControl != null) mobileControl.enabled = false;

            activateButton.gameObject.SetActive(true);
            deactivateButton.gameObject.SetActive(false);
        }
    }
}
