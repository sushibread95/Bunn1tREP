using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class CableConnectorAA : MonoBehaviour
{
    public float snapSpeed = 5f;
    public GameObject successVFX;
    public GameObject errorVFX;
    public VideoPlayer videoPlayer;
    public Canvas videoCanvas;
    public CanvasGroup fadeCanvas;
    public CablePuzzleAA puzzleManager;

    private Transform correctTarget;
    private bool isDragging = false;
    private Camera cam;
    private Vector3 startPosition;

    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
    }

    void Update()
    {
        #region Mobile Drag Support

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = cam.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && Physics.Raycast(ray, out hit))
                    {
                        transform.position = hit.point;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                    {
                        isDragging = false;

                        // Verifica se não conectou e retorna
                        if (correctTarget == null)
                        {
                            StartCoroutine(ReturnToStart());
                        }
                    }
                    break;
            }
        }

        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gameObject.tag))
        {
            correctTarget = other.transform;
            StartCoroutine(SnapToTarget(correctTarget));
        }
        else if (other.CompareTag("Verde") || other.CompareTag("Azul") || other.CompareTag("Vermelho") || other.CompareTag("Amarelo"))
        {
            Instantiate(errorVFX, transform.position, Quaternion.identity);
        }
    }

    IEnumerator SnapToTarget(Transform target)
    {
        // Notifica o gerenciador
        if (puzzleManager != null)
        {
            puzzleManager.OnCableConnected();
        }

        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * snapSpeed);
            yield return null;
        }

        transform.position = target.position;

        Instantiate(successVFX, transform.position, Quaternion.identity);

        if (videoPlayer != null)
        {
            videoCanvas.enabled = true;
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            videoCanvas.enabled = false;
        }

        if (fadeCanvas != null)
        {
            yield return StartCoroutine(FadeIn());

            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator ReturnToStart()
    {
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * snapSpeed);
            yield return null;
        }

        transform.position = startPosition;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadeCanvas.alpha = t;
            yield return null;
        }
    }
}
