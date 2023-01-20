using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {

        private PlayerManager _playerManager;
        private Vector2 _movement;

        public PlayerManager PlayerManager { set => _playerManager = value; }

        public void OnMove(InputAction.CallbackContext obj)
        {
            _movement = obj.ReadValue<Vector2>();
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            _playerManager.Rigidbody.velocity = new Vector2(_movement.x, _movement.y) * _playerManager.PlayerSpecs.PlayerSpeed * 200;
        }
    }
}