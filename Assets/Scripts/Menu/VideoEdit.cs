using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoEdit : MonoBehaviour
{
    [SerializeField] private UnityEngine.Video.VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _canvas;

    private void Start()
    {
        _videoPlayer.loopPointReached += OnFinish;
    }

    void OnFinish(UnityEngine.Video.VideoPlayer vp)
    {
        _canvas.SetActive(true);
        vp.gameObject.SetActive(false);
    }
}
