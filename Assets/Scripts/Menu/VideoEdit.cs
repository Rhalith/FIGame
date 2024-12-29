using UnityEngine;

namespace Scripts.Menu
{
    public class VideoEdit : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Video.VideoPlayer _videoPlayer;
        [SerializeField] private GameObject _menuCanvas, _introCanvas;
        [SerializeField] private BackgroundMusic _bgMusic;

        private void Start()
        {
            _videoPlayer.loopPointReached += OnFinish;
        }

        private void OnFinish(UnityEngine.Video.VideoPlayer vp)
        {
            FinishVideo();
        }

        public void OnSkip()
        {
            FinishVideo();
        }

        private void FinishVideo()
        {
            _menuCanvas.SetActive(true);
            _bgMusic.PlayMusic();
            _introCanvas.SetActive(false);
        }
    }
}