using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    public CinemachineCamera zoneCamera;
    public CameraZoneManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.SetCurrentCamera(zoneCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ClearCurrentCamera();
        }
    }
}