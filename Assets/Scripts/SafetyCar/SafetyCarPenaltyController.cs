using Scripts.EventBus;
using Scripts.Events;
using Scripts.Player;
using UnityEngine;

namespace Scripts.SafetyCar
{
    public class SafetyCarPenaltyController : MonoBehaviour
    {
        [SerializeField] private float damagePerSecond = 10f; // Amount of damage to apply per second

        private bool _playerInside;
        private float _damageTimer;
        private bool _canApplyDamage;
        private PlayerSpecs _playerSpecs;

        private void OnEnable()
        {
            EventBus<StartSafetyCarEvent>.AddListener(StartApplyingDamage);
            EventBus<EndSafetyCarEvent>.AddListener(StopApplyingDamage);
            EventBus<IncreaseDifficultyEvent>.AddListener(IncreaseDamage);
            EventBus<ResetSafetyCarEvent>.AddListener(ResetSafetyCar);
        }

        private void OnDisable()
        {
            EventBus<StartSafetyCarEvent>.RemoveListener(StartApplyingDamage);
            EventBus<EndSafetyCarEvent>.RemoveListener(StopApplyingDamage);
            EventBus<IncreaseDifficultyEvent>.RemoveListener(IncreaseDamage);
            EventBus<ResetSafetyCarEvent>.RemoveListener(ResetSafetyCar);
        }

        private void StartApplyingDamage(object sender, StartSafetyCarEvent @event)
        {
            _canApplyDamage = true;
        }

        private void StopApplyingDamage(object sender, EndSafetyCarEvent @event)
        {
            _canApplyDamage = false;
        }
        
        private void IncreaseDamage(object sender, IncreaseDifficultyEvent @event)
        {
            damagePerSecond *= @event.IncreaseRate;
            damagePerSecond = Mathf.Round(damagePerSecond * 10f) / 10f;
        }
        
        private void ResetSafetyCar(object sender, ResetSafetyCarEvent @event)
        {
            _canApplyDamage = false;
            _damageTimer = 0;
        }
        private void Update()
        {
            if(!_canApplyDamage) return;
            if (!_playerInside)
            {
                ApplyDamage();
            }
            else
            {
                _damageTimer = 0;
            }
        }

        private void ApplyDamage()
        {
            _damageTimer += Time.deltaTime;

            if (_damageTimer >= 1f)
            {
                _playerSpecs.DamagePlayer(damagePerSecond);
                _damageTimer = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if(_playerSpecs == null)
                    _playerSpecs = other.GetComponent<PlayerSpecs>();
                _playerInside = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInside = false;
            }
        }
    }
}