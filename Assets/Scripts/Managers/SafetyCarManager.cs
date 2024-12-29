using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Managers
{
    public class SafetyCarManager : MonoBehaviour
    {
        [SerializeField] private int _safetyCarProbability;
        [SerializeField] private float _safetyCarCooldown; // Cooldown period in seconds
        
        private bool _canCallSafetyCar;
        private float _lastSafetyCarCallTime = -Mathf.Infinity;

        private void OnEnable()
        {
            EventBus<ChangeSafetyCarStatusEvent>.AddListener(ChangeSafetyCarStatus);
        }
        
        private void OnDisable()
        {
            EventBus<ChangeSafetyCarStatusEvent>.RemoveListener(ChangeSafetyCarStatus);
        }

        private void ChangeSafetyCarStatus(object sender, ChangeSafetyCarStatusEvent @event)
        {
            _canCallSafetyCar = @event.CanSafetyCarBeDeployed;
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
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = true});
            EventBus<StartSafetyCarEvent>.Emit(this, new StartSafetyCarEvent());
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
        }
    }
}
