using Scripts.EventBus;
using Scripts.Events;
using Scripts.Masi;
using UnityEngine;

namespace Scripts.Managers
{
   public class MasiManager : MonoBehaviour
   {
      [SerializeField] private MasiAttack _masiAttack;
      [SerializeField] private MasiAnimation _masiAnimation;

      private void OnEnable()
      {
         EventBus<PitLaneEntranceEvent>.AddListener(StartComing);
         EventBus<PlayerDeathEvent>.AddListener(StopMasiAttack);
         EventBus<TimeEndEvent>.AddListener(StopMasiAttack);
         EventBus<CallSafetyCarEvent>.AddListener(SendMasi);
         EventBus<SendSafetyCarEvent>.AddListener(CallMasi);
      }

      private void OnDisable()
      {
         EventBus<PitLaneEntranceEvent>.RemoveListener(StartComing);
         EventBus<PlayerDeathEvent>.RemoveListener(StopMasiAttack);
         EventBus<TimeEndEvent>.RemoveListener(StopMasiAttack);
         EventBus<CallSafetyCarEvent>.RemoveListener(SendMasi);
         EventBus<SendSafetyCarEvent>.RemoveListener(CallMasi);
      }

      private void SendMasi(object sender, CallSafetyCarEvent @event)
      {
         StopAttack();
         StartGoing();
      }

      private void CallMasi(object sender, SendSafetyCarEvent @event)
      {
         StartComing();
      }

      private void StopMasiAttack(object sender, TimeEndEvent @event)
      {
         StopAttack();
         StartGoing();
      }

      private void StopMasiAttack(object sender, PlayerDeathEvent @event)
      {
         StopAttack();
      }

      private void StartComing(object sender, PitLaneEntranceEvent @event)
      {
         if (!@event.IsEntering && !GameManager.Instance.IsGameFinished)
         {
            StartComing();
         }
      }

      private void StartComing()
      {
         if(!GameManager.Instance.IsGameFinished)
            _masiAnimation.StartComing();
      }

      private void StartGoing()
      {
         _masiAnimation.StartGoing();
      }
      public void StartAttack()
      {
         if(GameManager.Instance.IsGameFinished) return;
         _masiAttack.StartShooting();
         _masiAnimation.StartFire();
         EventBus<ChangeSafetyCarStatusEvent>.Emit(this, new ChangeSafetyCarStatusEvent { CanSafetyCarBeDeployed = true });
      }
      public void StopAttack()
      {
         _masiAttack.StopShooting();
         _masiAnimation.StopFire();
         EventBus<ChangeSafetyCarStatusEvent>.Emit(this, new ChangeSafetyCarStatusEvent { CanSafetyCarBeDeployed = false });
      }
   }
}
