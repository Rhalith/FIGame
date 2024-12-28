using Scripts.EventBus;
using Scripts.Events;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.Player
{
   public class PlayerSpecs : MonoBehaviour
   {
      private float _playerHealth = 100;

      public float PlayerSpeed { get; private set; }


      private void OnEnable()
      {
         EventBus<ChangeTireEvent>.AddListener(ChangeTire);
      }

      private void OnDisable()
      {
         EventBus<ChangeTireEvent>.RemoveListener(ChangeTire);
      }

      private void ChangeTire(object sender, ChangeTireEvent @event)
      {
         ResetPlayerHealth();
         ChangePlayerSpeed(@event.ChosenTire);
      }


      public void DamagePlayer(float damage)
      {
         if (_playerHealth > 0)
         {
            _playerHealth -= damage;
            GameManager.Instance.Healthbar.SetHealth(_playerHealth);
         }
         else
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

      private void StopPlayerMovement()
      {
         PlayerSpeed = 0;
      }
   }
}