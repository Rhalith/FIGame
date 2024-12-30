using Scripts.EventBus;
using Scripts.Events;
using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
   public class PlayerSpecs : MonoBehaviour
   {
      private static readonly Dictionary<Tire, float> tireMaxHealthList = new()
      {
         { Tire.Soft, 100 },
         { Tire.Medium, 150 },
         { Tire.Hard, 200 }
      };

      private float _maxHealth;
      private float _playerHealth;
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
         EventBus<UpdateScoreEvent>.Emit(this, new UpdateScoreEvent { ScoreChange = _playerHealth });
      }

      private void ChangeTire(object sender, ChangeTireEvent @event)
      {
         ChangePlayerSpeed(@event.ChosenTire);
         ChangePlayerHealthRate(@event.ChosenTire);
      }


      public void DamagePlayer(float damage)
      {
         SetPlayerHealth(_playerHealth - damage);

         if (_playerHealth <= 0 && !GameManager.Instance.IsGameFinished)
         {
            EventBus<PlayerDeathEvent>.Emit(this, new PlayerDeathEvent());
            EventBus<DisablePenaltiesEvent>.Emit(this, new DisablePenaltiesEvent());
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = false});
            GameManager.Instance.IsGameFinished = true;
            GameManager.Instance.CanPause = false;
         }
      }

      public void HealPlayer(float heal)
      {
         SetPlayerHealth(_playerHealth + heal);
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
         _maxHealth = tireMaxHealthList[tire];
         SetPlayerHealth(_maxHealth);
      }

      private void SetPlayerHealth(float health)
      {
         if (health < 0)
         {
            health = 0;
         }
         else if (health > _maxHealth)
         {
            health = _maxHealth;
         }

         _playerHealth = health;
         EventBus<ChangeHealthEvent>.Emit(this, new ChangeHealthEvent { UpdatedHealth = _playerHealth, MaxHealth = _maxHealth });
      }
   }
}