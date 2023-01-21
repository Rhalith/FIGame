using Assets.Scripts.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        #region Player Scripts
        [Header("Player Scripts")]
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerDeath _playerDeath;
        [SerializeField] private PlayerSpecs _playerSpecs;
        [SerializeField] private PlayerMovement _playerMovement;
        #endregion
        #region Other Components
        [Header("Other Components")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private ScrollBackground _scrollBackground;
        [SerializeField] private GameObject _tireMenu;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GameObject _deathScreen;
        #endregion

        #region Getters and Setters
        public PlayerAnimation PlayerAnimation { get => _playerAnimation; set => _playerAnimation = value; }
        public PlayerDeath PlayerDeath { get => _playerDeath; set => _playerDeath = value; }
        public PlayerSpecs PlayerSpecs { get => _playerSpecs; set => _playerSpecs = value; }
        public PlayerMovement PlayerMovement { get => _playerMovement; set => _playerMovement = value; }
        public ScrollBackground ScrollBackground { get => _scrollBackground; }
        public GameObject TireMenu { get => _tireMenu; }
        public Animator Animator { get => _animator; }
        public Rigidbody2D Rigidbody { get => _rigidbody; }
        public GameObject DeathScreen { get => _deathScreen; }
        public GameManager GameManager { get => _gameManager; }

        #endregion

        #region Fields
        public delegate void PlayerAction();
        public PlayerAction OnPlayerReset;
        #endregion

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
            _playerMovement.PlayerManager = this;
        }

        public void OnDeath()
        {
            _playerDeath.KillPlayer();
        }

        public void OnReset()
        {
            OnPlayerReset?.Invoke();
        }
    }
}