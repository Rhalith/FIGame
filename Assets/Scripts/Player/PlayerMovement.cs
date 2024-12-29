using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerSpecs _playerSpecs;
        [SerializeField] private Rigidbody2D _rigidbody;
        private Vector2 _movement;
        
        private Vector2 _startPosition;
        
        private bool _shouldReset;
        private bool _canMove;
        
        private void Start()
        {
            _startPosition = transform.position;
        }

        private void OnEnable()
        {
            EventBus<ResetCarPositionEvent>.AddListener(ResetCarPosition);
            EventBus<StartTimerEvent>.AddListener(StartMovement);
            EventBus<TimeEndEvent>.AddListener(StopMovement);
            EventBus<CallSafetyCarEvent>.AddListener(StopMovement);
            EventBus<StartSafetyCarEvent>.AddListener(StartMovement);
        }
        
        private void OnDisable()
        {
            EventBus<ResetCarPositionEvent>.RemoveListener(ResetCarPosition);
            EventBus<StartTimerEvent>.RemoveListener(StartMovement);
            EventBus<TimeEndEvent>.RemoveListener(StopMovement);
            EventBus<CallSafetyCarEvent>.RemoveListener(StopMovement);
            EventBus<StartSafetyCarEvent>.RemoveListener(StartMovement);
        }

        private void StopMovement(object sender, CallSafetyCarEvent @event)
        {
            _canMove = false;
        }

        private void StartMovement(object sender, StartSafetyCarEvent @event)
        {
            _canMove = true;
        }

        private void StartMovement(object sender, StartTimerEvent @event)
        {
            _canMove = true;
        }

        private void StopMovement(object sender, TimeEndEvent @event)
        {
            _canMove = false;
        }

        private void ResetCarPosition(object sender, ResetCarPositionEvent @event)
        {
            _shouldReset = true;
        }

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
            if (_shouldReset)
            {
                _rigidbody.velocity = Vector2.zero;
                transform.DOMove(_startPosition, 0.5f);
                _shouldReset = false;
            }
            else
            {
                if(!_canMove) _rigidbody.velocity = Vector2.zero;
                else _rigidbody.velocity = new Vector2(_movement.x, _movement.y) * (_playerSpecs.PlayerSpeed * 200);
            }
        }
    }
}