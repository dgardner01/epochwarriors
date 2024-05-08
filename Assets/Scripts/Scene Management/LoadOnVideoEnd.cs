using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadOnVideoEnd : MonoBehaviour
{
    VideoPlayer video => GetComponent<VideoPlayer>();
    // Start is called before the first frame update
    void Awake()
    {
        video.loopPointReached += CheckOver;
    }

    // Update is called once per frame
    void CheckOver(VideoPlayer vp)
    {
        SceneManager.LoadScene("BaseFight");
    }
}
