using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerSpecs _playerSpecs;
        private Vector2 _movement;


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
            _rigidbody.velocity = new Vector2(_movement.x, _movement.y) * _playerSpecs.PlayerSpeed * 200;
        }
    }
}