using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private MasiManager _masiManager;

        public MasiManager MasiManager { set => _masiManager = value; }
        public PlayerManager PlayerManager { set => _playerManager = value; }

        public void KillPlayer()
        {
            _masiManager.StopAttack();
            _playerManager.PlayerSpecs.StopPlayerMovement();
        }
    }
}