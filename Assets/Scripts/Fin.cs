using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Fin : MonoBehaviour
{


    public VideoPlayer m_videoPlayer = null;

    private void Awake()
    {
        m_videoPlayer = GetComponent<VideoPlayer>();
        m_videoPlayer.loopPointReached += MustGoToNextScene;
    }

    void MustGoToNextScene(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Credits");
    }
}