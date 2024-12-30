using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerSpecs _playerSpecs;
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private float _xBound;
        [SerializeField] private float _yBound;
        [SerializeField] private float _safetyCarXBound;
        private Vector2 _movement;

        private Vector2 _startPosition;

        private bool _shouldReset;
        private bool _canMove;
        private bool _safetyCarBoundsActive;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void OnEnable()
        {
            EventBus<ResetCarPositionEvent>.AddListener(ResetCarPosition);
            EventBus<SetPlayerMovementEvent>.AddListener(ChangeMovement);
            EventBus<CallSafetyCarEvent>.AddListener(ActivateSafetyCarBounds);
            EventBus<SendSafetyCarEvent>.AddListener(DeactivateSafetyCarBounds);
        }

        private void OnDisable()
        {
            EventBus<ResetCarPositionEvent>.RemoveListener(ResetCarPosition);
            EventBus<SetPlayerMovementEvent>.RemoveListener(ChangeMovement);
            EventBus<CallSafetyCarEvent>.RemoveListener(ActivateSafetyCarBounds);
            EventBus<SendSafetyCarEvent>.RemoveListener(DeactivateSafetyCarBounds);
        }

        private void ChangeMovement(object sender, SetPlayerMovementEvent @event)
        {
            _canMove = @event.CanMove;
        }

        private void ResetCarPosition(object sender, ResetCarPositionEvent @event)
        {
            _shouldReset = true;
        }

        private void ActivateSafetyCarBounds(object sender, CallSafetyCarEvent @event)
        {
            _safetyCarBoundsActive = true;
        }

        private void DeactivateSafetyCarBounds(object sender, SendSafetyCarEvent @event)
        {
            _safetyCarBoundsActive = false;
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
                if (!_canMove)
                {
                    _rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    Vector2 newVelocity = new Vector2(_movement.x, _movement.y) * (_playerSpecs.PlayerSpeed * 200);
                    _rigidbody.velocity = newVelocity;
                    float maxX = _safetyCarBoundsActive ? _safetyCarXBound : _xBound;

                    Vector3 clampedPosition = new Vector3(
                        Mathf.Clamp(transform.localPosition.x, -_xBound, maxX),
                        Mathf.Clamp(transform.localPosition.y, -_yBound, _yBound),
                        transform.localPosition.z
                    );

                    transform.localPosition = clampedPosition;
                }
            }
        }
    }
}