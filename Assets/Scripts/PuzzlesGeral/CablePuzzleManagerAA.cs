using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class CablePuzzleAA : MonoBehaviour
{
    public int totalCables = 4;
    private int connectedCount = 0;

    public VideoPlayer videoPlayer;
    public Canvas videoCanvas;
    public GameObject barrierToDisable; // Ex: portão/bloqueio
    public CanvasGroup fadeCanvas;

    public float fadeDuration = 1f;

    public void OnCableConnected()
    {
        connectedCount++;

        if (connectedCount >= totalCables)
        {
            StartCoroutine(PlayVideoAndUnlock());
        }
    }

    IEnumerator PlayVideoAndUnlock()
    {
        // Fade in (opcional)
        if (fadeCanvas != null)
            yield return StartCoroutine(Fade(0, 1));

        // Toca o vídeo
        if (videoPlayer != null)
        {
            videoCanvas.enabled = true;
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
                yield return null;

            videoCanvas.enabled = false;
        }

        // Desativa barreira
        if (barrierToDisable != null)
            barrierToDisable.SetActive(false);

        // Fade out (opcional)
        if (fadeCanvas != null)
            yield return StartCoroutine(Fade(1, 0));
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
