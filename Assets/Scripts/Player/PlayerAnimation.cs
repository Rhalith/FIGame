using DG.Tweening;
using Scripts.Masi;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private MasiManager _masiManager;
    
        public MasiManager MasiManager { set => _masiManager = value; }
        public PlayerManager PlayerManager { set => _playerManager = value; }


        public void EnterPitLane()
        {
            _playerManager.Animator.enabled = true;
            DOTween.To(() => _playerManager.ScrollBackground.Speed, x => _playerManager.ScrollBackground.Speed = x, 0.2f, 1f);
        }

        public void StartPitStop()
        {
            DOTween.To(() => _playerManager.ScrollBackground.Speed, x => _playerManager.ScrollBackground.Speed = x, 0, 1f);
            _playerManager.TireMenu.SetActive(true);
        }

        public void AfterPitStop()
        {
            DOTween.To(() => _playerManager.ScrollBackground.Speed, x => _playerManager.ScrollBackground.Speed = x, 0.2f, 1f);
            _playerManager.Animator.SetBool("isTireSelected", true);
            _playerManager.TireMenu.SetActive(false);
        }

        public void ExitPitLane()
        {
            DOTween.To(() => _playerManager.ScrollBackground.Speed, x => _playerManager.ScrollBackground.Speed = x, 0.4f, 1f);
            _playerManager.Animator.SetBool("isTireSelected", false);
            _masiManager.StartComing();
            _playerManager.Animator.enabled = false;
        }
    }
}
