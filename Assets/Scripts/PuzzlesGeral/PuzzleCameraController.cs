using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class PuzzleCameraController : MonoBehaviour
{
    [Header("Referências")]
    public CinemachineCamera puzzleCamera;
    public Button activateButton;
    public Button deactivateButton;
    public PuzzleLuzManager puzzleManager;

    [Header("HUD do jogador")]
    public GameObject[] playerHUDObjects;

    [Header("Prioridades da câmera")]
    public int activePriority = 20;
    public int inactivePriority = 0;

    [Header("Configurações de camada")]
    public string playerLayerName = "Player";

    private bool playerInZone = false;
    private bool puzzleActive = false;

    private void Start()
    {
        if (activateButton != null) activateButton.gameObject.SetActive(false);
        if (deactivateButton != null) deactivateButton.gameObject.SetActive(false);

        if (activateButton != null) activateButton.onClick.AddListener(ActivatePuzzle);
        if (deactivateButton != null) deactivateButton.onClick.AddListener(DeactivatePuzzle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (!puzzleActive && activateButton != null)
                activateButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            DeactivatePuzzle();
            if (activateButton != null) activateButton.gameObject.SetActive(false);
            if (deactivateButton != null) deactivateButton.gameObject.SetActive(false);
        }
    }

    private void ActivatePuzzle()
    {
        if (!playerInZone || puzzleCamera == null) return;

        puzzleCamera.Priority = activePriority;
        puzzleActive = true;

        if (activateButton != null) activateButton.gameObject.SetActive(false);
        if (deactivateButton != null) deactivateButton.gameObject.SetActive(true);

        TogglePlayerHUD(false);
        SetPlayerVisibility(false);

        if (puzzleManager != null)
            puzzleManager.StartPuzzle();

        Debug.Log("[Puzzle] Puzzle ativado.");
    }

    private void DeactivatePuzzle()
    {
        if (puzzleCamera == null || !puzzleActive) return;

        puzzleCamera.Priority = inactivePriority;
        puzzleActive = false;

        if (playerInZone && activateButton != null)
            activateButton.gameObject.SetActive(true);
        if (deactivateButton != null)
            deactivateButton.gameObject.SetActive(false);

        TogglePlayerHUD(true);
        SetPlayerVisibility(true);

        if (puzzleManager != null)
            puzzleManager.StopPuzzle();

        Debug.Log("[Puzzle] Puzzle desativado.");
    }

    private void TogglePlayerHUD(bool state)
    {
        foreach (GameObject hud in playerHUDObjects)
        {
            if (hud != null)
                hud.SetActive(state);
        }
    }

    private void SetPlayerVisibility(bool visible)
    {
        Camera mainCam = Camera.main;
        int playerLayer = LayerMask.NameToLayer(playerLayerName);

        if (mainCam != null && playerLayer >= 0)
        {
            if (visible)
                mainCam.cullingMask |= (1 << playerLayer);
            else
                mainCam.cullingMask &= ~(1 << playerLayer);
        }
    }

    // ? Chamado pelo PuzzleLuzManager ao final do puzzle
    public void ForceEndPuzzle()
    {
        puzzleCamera.Priority = inactivePriority;
        TogglePlayerHUD(true);
        SetPlayerVisibility(true);
        puzzleActive = false;

        if (activateButton != null) activateButton.gameObject.SetActive(false);
        if (deactivateButton != null) deactivateButton.gameObject.SetActive(false);

        // Desativa o collider para impedir nova entrada
        GetComponent<Collider>().enabled = false;

        Debug.Log("[Puzzle] Puzzle finalizado permanentemente.");
    }
}
