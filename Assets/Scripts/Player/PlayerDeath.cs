using DG.Tweening;
using Scripts.Masi;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private MasiManager _masiManager;
        private Tween myTween = null;
        public MasiManager MasiManager { set => _masiManager = value; }
        public PlayerManager PlayerManager { set => _playerManager = value; }

        public void KillPlayer()
        {
            _masiManager.StopAttack();
            _playerManager.PlayerSpecs.StopPlayerMovement();
            myTween = DOTween.To(() => _playerManager.ScrollBackground.Speed, x => _playerManager.ScrollBackground.Speed = x, 0, 1f).OnComplete(ActivateDeathScreen);
        }
        private void ActivateDeathScreen()
        {
            _playerManager.DeathScreen.SetActive(true);
        }
    }
}