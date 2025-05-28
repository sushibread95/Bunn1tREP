using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Associe aqui o último vídeo
    public string sceneToLoad;      // Nome da próxima cena

    void Start()
    {
        videoPlayer.loopPointReached += LoadNextScene;
    }

    void LoadNextScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
