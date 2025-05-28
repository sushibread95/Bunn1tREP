using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoFadeController : MonoBehaviour
{
    [SerializeField] private RawImage telaPreta;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float fadeDuration = 1f;

    void Start()
    {
        telaPreta.gameObject.SetActive(true);
        telaPreta.color = new Color(0, 0, 0, 1);

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare(); // começa a carregar o vídeo
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.Play();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = telaPreta.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            telaPreta.color = new Color(0, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        telaPreta.color = new Color(0, 0, 0, 0);
        telaPreta.gameObject.SetActive(false);
    }
}
