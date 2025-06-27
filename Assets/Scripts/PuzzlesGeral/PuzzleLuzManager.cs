using UnityEngine;
using UnityEngine.Video;
using Unity.Cinemachine;
using System.Collections;

public class PuzzleLuzManager : MonoBehaviour
{
    [Header("Câmera do puzzle")]
    public CinemachineCamera puzzleCamera;

    [Header("HUD e objetos para esconder durante o puzzle")]
    public GameObject[] objectsToTemporarilyDisable;

    [Header("Conexões")]
    public int totalCables = 5;
    private int connectedCount = 0;
    private bool puzzleCompleted = false;

    [Header("Vídeo e Canvas")]
    public VideoPlayer videoPlayer;
    public GameObject canvasVideoParent; // ← Atribuir o "CanvasVideos" aqui no Inspector

    [Header("Resultado final")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    [Header("Collider do puzzle (para desativar após conclusão)")]
    public Collider puzzleTriggerCollider; // ← Arraste o collider do PuzzleCameraController aqui

    void Start()
    {
        puzzleCamera.Priority = 0;
        Debug.Log("[Puzzle] Sistema iniciado.");
    }

    public void StartPuzzle()
    {
        Debug.Log("[Puzzle] Iniciando lógica do puzzle...");
        this.enabled = true;
    }

    public void StopPuzzle()
    {
        Debug.Log("[Puzzle] Parando lógica do puzzle...");
        this.enabled = false;
    }

    public void EnterPuzzle()
    {
        puzzleCamera.Priority = 20;

        foreach (GameObject obj in objectsToTemporarilyDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"[Puzzle] HUD escondido: {obj.name}");
            }
        }
    }

    public void ExitPuzzle()
    {
        puzzleCamera.Priority = 0;

        foreach (GameObject obj in objectsToTemporarilyDisable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"[Puzzle] HUD reativado: {obj.name}");
            }
        }
    }

    public void CableConnected()
    {
        if (puzzleCompleted) return;

        connectedCount++;
        Debug.Log($"[Puzzle] Cabo conectado: {connectedCount}/{totalCables}");

        if (connectedCount >= totalCables)
        {
            Debug.Log("[Puzzle] Todos os cabos conectados. Finalizando...");
            StartCoroutine(PlayVideoAndUnlock());
        }
    }

    IEnumerator PlayVideoAndUnlock()
    {
        puzzleCompleted = true;

        // ✅ Ativa o Canvas com o vídeo antes de tocar
        if (canvasVideoParent != null && !canvasVideoParent.activeSelf)
        {
            canvasVideoParent.SetActive(true);
        }

        // ✅ Reproduz vídeo
        if (videoPlayer != null)
        {
            Debug.Log("[Puzzle] Reproduzindo vídeo...");
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
                yield return null;

            Debug.Log("[Puzzle] Vídeo concluído.");
        }

        // ✅ Ativa e desativa objetos finais
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // ✅ Desativa o collider do puzzle
        if (puzzleTriggerCollider != null)
        {
            puzzleTriggerCollider.enabled = false;
        }

        ExitPuzzle();

        Debug.Log("[Puzzle] Finalizado.");
    }
}
