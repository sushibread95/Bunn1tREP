using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSequenceSceneLoader : MonoBehaviour
{
    public VideoPlayer video1;
    public VideoPlayer video2;
    public string nextSceneName;

    void Start()
    {
        video1.loopPointReached += OnVideo1Finished;
        video2.loopPointReached += OnVideo2Finished;

        video1.Play();
    }

    void OnVideo1Finished(VideoPlayer vp)
    {
        video1.gameObject.SetActive(false); // esconde o vídeo 1 se estiver na mesma tela
        video2.gameObject.SetActive(true);  // mostra o vídeo 2
        video2.Play();
    }

    void OnVideo2Finished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
