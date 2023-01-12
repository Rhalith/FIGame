using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class VideoEdit : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Video.VideoPlayer _videoPlayer;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private BackgroundMusic _bgMusic;

        private void Start()
        {
            _videoPlayer.loopPointReached += OnFinish;
        }

        void OnFinish(UnityEngine.Video.VideoPlayer vp)
        {
            _canvas.SetActive(true);
            _bgMusic.PlayMusic();
            vp.gameObject.SetActive(false);
        }
    }
}