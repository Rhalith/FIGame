using Scripts.EventBus;
using Scripts.Events;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Scripts.Player
{
   public class PlayerSpecs : MonoBehaviour
   {
      private float _playerHealth = 100;
      private float _damageRate;
      public float PlayerSpeed { get; private set; }


      private void OnEnable()
      {
         EventBus<ChangeTireEvent>.AddListener(ChangeTire);
         EventBus<TimeEndEvent>.AddListener(UpdateScore);
      }

      private void OnDisable()
      {
         EventBus<ChangeTireEvent>.RemoveListener(ChangeTire);
         EventBus<TimeEndEvent>.RemoveListener(UpdateScore);
      }

      private void UpdateScore(object sender, TimeEndEvent @event)
      {
         EventBus<UpdateScoreEvent>.Emit(this, new UpdateScoreEvent {ScoreChange = _playerHealth});
      }

      private void ChangeTire(object sender, ChangeTireEvent @event)
      {
         ResetPlayerHealth();
         ChangePlayerSpeed(@event.ChosenTire);
         ChangePlayerHealthRate(@event.ChosenTire);
      }


      public void DamagePlayer(float damage)
      {
         if (_playerHealth > 0)
         {
            _playerHealth -= damage * _damageRate;
            EventBus<ChangeHealthEvent>.Emit(this, new ChangeHealthEvent {HealthChange = -damage * _damageRate});
         }
         if (_playerHealth <= 0)
         {
            EventBus<PlayerDeathEvent>.Emit(this, new PlayerDeathEvent());
            StopPlayerMovement();
         }
      }

      public void HealPlayer(float heal)
      {
         if (_playerHealth >= 100) { return; }

         _playerHealth += heal;
         GameManager.Instance.Healthbar.SetHealth(_playerHealth);
      }

      private void ResetPlayerHealth()
      {
         _playerHealth = 100;
      }

      private void ChangePlayerSpeed(Tire tire)
      {
         switch (tire)
         {
            case Tire.Soft:
               PlayerSpeed = 2.5f;
               break;
            case Tire.Medium:
               PlayerSpeed = 2f;
               break;
            case Tire.Hard:
               PlayerSpeed = 1.5f;
               break;
         }
      }
      
      private void ChangePlayerHealthRate(Tire tire)
      {
         switch (tire)
         {
            case Tire.Soft:
               _damageRate = 2f;
               break;
            case Tire.Medium:
               _damageRate = 1.5f;
               break;
            case Tire.Hard:
               _damageRate = 1f;
               break;
         }
      }

      private void StopPlayerMovement()
      {
         PlayerSpeed = 0;
      }
   }
}