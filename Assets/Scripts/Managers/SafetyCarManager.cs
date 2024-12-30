using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Managers
{
    public class SafetyCarManager : MonoBehaviour
    {
        [SerializeField] private int _safetyCarProbability;
        [SerializeField] private float _safetyCarCooldown;
        
        private bool _canCallSafetyCar;
        private float _lastSafetyCarCallTime = -Mathf.Infinity;

        private void OnEnable()
        {
            EventBus<ChangeSafetyCarStatusEvent>.AddListener(ChangeSafetyCarStatus);
            EventBus<ResetSafetyCarEvent>.AddListener(ResetSafetyCar);
        }
        
        private void OnDisable()
        {
            EventBus<ChangeSafetyCarStatusEvent>.RemoveListener(ChangeSafetyCarStatus);
            EventBus<ResetSafetyCarEvent>.RemoveListener(ResetSafetyCar);
        }

        private void ChangeSafetyCarStatus(object sender, ChangeSafetyCarStatusEvent @event)
        {
            _canCallSafetyCar = @event.CanSafetyCarBeDeployed;
        }
        
        private void ResetSafetyCar(object sender, ResetSafetyCarEvent @event)
        {
            _canCallSafetyCar = true;
            _lastSafetyCarCallTime = -Mathf.Infinity;
        }

        private void FixedUpdate()
        {
            // Check cooldown and probability
            if (_canCallSafetyCar && Time.time - _lastSafetyCarCallTime >= _safetyCarCooldown)
            {
                if (Random.Range(0, 1000) < _safetyCarProbability)
                {
                    EmitCallSafetyCarEvent();
                }
            }
        }

        private void EmitCallSafetyCarEvent()
        {
            _canCallSafetyCar = false;
            _lastSafetyCarCallTime = Time.time; // Record the time of this call
            EventBus<CallSafetyCarEvent>.Emit(this, new CallSafetyCarEvent());
            EventBus<DisablePenaltiesEvent>.Emit(this, new DisablePenaltiesEvent());
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = false});
        }
        
        public void StartSafetyCarMovement()
        {
            GameManager.Instance.CanPause = true;
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = true});
            EventBus<StartSafetyCarEvent>.Emit(this, new StartSafetyCarEvent());
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
        }
    }
}
