using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.SafetyCar
{
   public class SafetyCarController : MonoBehaviour
   {
      [SerializeField] private float speed = 3f;
      [SerializeField] private float maxY;
      [SerializeField] private float minY;

      [SerializeField] private Vector2 _startPosition;
      [SerializeField] private Vector2 _endPosition;

      private bool _shouldMove;
      private bool _movingToEnd;
      private bool _oscillating;
      private bool _movingToStart;

      private Vector3 _currentTarget;

      private void Start()
      {
         speed *= 100;
         transform.localPosition = new Vector3(_startPosition.x, _startPosition.y, transform.localPosition.z);
         _currentTarget = transform.localPosition;
      }

      private void OnEnable()
      {
         EventBus<CallSafetyCarEvent>.AddListener(HandleCallSafetyCar);
         EventBus<StartSafetyCarEvent>.AddListener(HandleStartSafetyCar);
         EventBus<EndSafetyCarEvent>.AddListener(HandleEndSafetyCar);
         EventBus<ResetSafetyCarEvent>.AddListener(ResetSafetyCar);
      }

      private void OnDisable()
      {
         EventBus<CallSafetyCarEvent>.RemoveListener(HandleCallSafetyCar);
         EventBus<StartSafetyCarEvent>.RemoveListener(HandleStartSafetyCar);
         EventBus<EndSafetyCarEvent>.RemoveListener(HandleEndSafetyCar);
         EventBus<ResetSafetyCarEvent>.RemoveListener(ResetSafetyCar);
      }

      private void Update()
      {
         if (_shouldMove)
         {
            MoveTowardsTarget();
         }
      }

      private void HandleCallSafetyCar(object sender, CallSafetyCarEvent @event)
      {
         _shouldMove = true;
         _oscillating = false;
         _currentTarget = new Vector3(_endPosition.x, _endPosition.y, transform.localPosition.z);
      }

      private void HandleStartSafetyCar(object sender, StartSafetyCarEvent @event)
      {
         _shouldMove = true;
         _oscillating = true;
         _currentTarget = new Vector3(transform.localPosition.x, UnityEngine.Random.value > 0.5f ? maxY : minY, transform.localPosition.z);
      }

      private void HandleEndSafetyCar(object sender, EndSafetyCarEvent @event)
      {
         _shouldMove = true;
         _oscillating = false;
         _movingToEnd = true;
         _currentTarget = new Vector3(_endPosition.x, _endPosition.y, transform.localPosition.z);
      }
      
      
      private void ResetSafetyCar(object sender, ResetSafetyCarEvent @event)
      {
         transform.localPosition = new Vector3(_startPosition.x, _startPosition.y, transform.localPosition.z);
         _currentTarget = transform.localPosition;
         _oscillating = false;
         _movingToEnd = false;
         _movingToStart = false;
         _shouldMove = false;
      }

      private void MoveTowardsTarget()
      {
         transform.localPosition = Vector3.MoveTowards(transform.localPosition, _currentTarget, speed * Time.deltaTime);

         if (Vector3.Distance(transform.localPosition, _currentTarget) < 0.01f)
         {
            OnTargetReached();
         }
      }

      private void OnTargetReached()
      {
         if (_movingToEnd)
         {
            _movingToEnd = false;
            _movingToStart = true;
            _currentTarget = new Vector3(_startPosition.x, _startPosition.y, transform.localPosition.z);

            EventBus<SendSafetyCarEvent>.Emit(this, new SendSafetyCarEvent());
         }
         else if (_movingToStart)
         {
            _shouldMove = false;
            _movingToStart = false;
         }
         else if (_oscillating)
         {
            _currentTarget = new Vector3(transform.localPosition.x, _currentTarget.y == maxY ? minY : maxY, transform.localPosition.z);
         }
      }
   }
}
