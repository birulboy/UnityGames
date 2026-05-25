using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    IEnumerator Start()
    {
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.Prepare();

        float timeout = 0f;
        while (!videoPlayer.isPrepared && timeout < 10f)
        {
            timeout += Time.deltaTime;
            yield return null;
        }

        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
            if (audioSource != null)
                //audioSource.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            // Si no prepara en 10s igual pasa a la siguiente escena
            SceneManager.LoadScene(2);
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(2);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(2);
        }
    }
}