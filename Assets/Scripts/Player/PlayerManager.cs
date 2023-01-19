using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerDeath _playerDeath;
        [SerializeField] private PlayerSpecs _playerSpecs;
        [SerializeField] private PlayerMovement _playerMovement;

        public PlayerAnimation PlayerAnimation { get => _playerAnimation; set => _playerAnimation = value; }
        public PlayerDeath PlayerDeath { get => _playerDeath; set => _playerDeath = value; }
        public PlayerSpecs PlayerSpecs { get => _playerSpecs; set => _playerSpecs = value; }
        public PlayerMovement PlayerMovement { get => _playerMovement; set => _playerMovement = value; }

        public void SetMasiManager(MasiManager masiManager)
        {
            _playerAnimation.MasiManager = masiManager;
            _playerDeath.MasiManager = masiManager;
        }
        public void SetPlayerManager()
        {
            _playerAnimation.PlayerManager = this;
            _playerDeath.PlayerManager = this;
            _playerSpecs.PlayerManager = this;
        }

        public void OnDeath()
        {
            _playerDeath.KillPlayer();
        }
    }
}