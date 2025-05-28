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

    [Header("Vídeo e fade")]
    public VideoPlayer videoPlayer;
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    [Header("Resultado final")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    void Start()
    {
        puzzleCamera.Priority = 0;
        Debug.Log("[Puzzle] Sistema iniciado.");
    }
    public void StartPuzzle()
    {
        Debug.Log("[Puzzle] Iniciando lógica do puzzle...");
        // Aqui você pode ativar UI, objetos de interação, etc.
        this.enabled = true; // Se quiser começar a lógica por Update()
    }

    public void StopPuzzle()
    {
        Debug.Log("[Puzzle] Parando lógica do puzzle...");
        // Aqui você pode esconder UI, travar inputs etc.
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

        if (fadeCanvas != null)
        {
            Debug.Log("[Puzzle] Iniciando fade-in...");
            yield return StartCoroutine(Fade(0, 1));
        }

        if (videoPlayer != null)
        {
            Debug.Log("[Puzzle] Reproduzindo vídeo...");
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
                yield return null;

            Debug.Log("[Puzzle] Vídeo concluído.");
        }

        if (fadeCanvas != null)
        {
            Debug.Log("[Puzzle] Iniciando fade-out...");
            yield return StartCoroutine(Fade(1, 0));
        }

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

        ExitPuzzle();

        Debug.Log("[Puzzle] Finalizado.");
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeCanvas.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }
}
