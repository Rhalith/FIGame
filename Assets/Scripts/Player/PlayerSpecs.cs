using Assets.Scripts.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSpecs : MonoBehaviour
    {
        [SerializeField] private Healthbar _healthBar;

        private PlayerManager _playerManager;
        private float _playerHealth = 100;
        private float _playerSpeed;

        public float PlayerSpeed { get => _playerSpeed; }
        public float PlayerHealth { get => _playerHealth; }
        public PlayerManager PlayerManager { set => _playerManager = value; }

        public void DamagePlayer(float damage)
        {
            if (_playerHealth > 0)
            {
                _playerHealth -= damage;
                _healthBar.SetHealth(_playerHealth);
            }
            else
            {
                _playerManager.OnDeath();
            }
        }

        public void ResetPlayerHealth()
        {
            _playerHealth = 100;
        }

        public void ChangePlayerSpeed(Tire tire)
        {
            switch (tire)
            {
                case Tire.Soft:
                    _playerSpeed = 2.5f;
                    break;
                case Tire.Medium:
                    _playerSpeed = 2f;
                    break;
                case Tire.Hard:
                    _playerSpeed = 1.5f;
                    break;
            }
        }

        public void StopPlayerMovement()
        {
            _playerSpeed = 0;
        }
    }
}