using Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerSpecs _playerSpecs;
        [SerializeField] private Rigidbody2D _rigidbody;
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
            _rigidbody.velocity = new Vector2(_movement.x, _movement.y) * (_playerSpecs.PlayerSpeed * 200);
        }
    }
}