using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class SingleVideoSceneLoader : MonoBehaviour
{
    public VideoPlayer video;
    public string nextSceneName;
    public Image fadeImage;           // UI Image que cobre a tela
    public float fadeDuration = 1f;   // Duração do fade

    void Start()
    {
        if (video == null || fadeImage == null)
        {
            Debug.LogError("VideoPlayer ou FadeImage não foi atribuído.");
            return;
        }

        video.isLooping = false;
        fadeImage.color = new Color(0, 0, 0, 0); // Começa transparente

        video.loopPointReached += OnVideoFinished;
        video.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
