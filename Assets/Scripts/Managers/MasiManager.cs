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
        }
        
        private void OnDisable()
        {
            EventBus<PitLaneEntranceEvent>.RemoveListener(StartComing);
            EventBus<PlayerDeathEvent>.RemoveListener(StopMasiAttack);
            EventBus<TimeEndEvent>.RemoveListener(StopMasiAttack);
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
            if (!@event.IsEntering)
            {
                StartComing();
            }
        }

        private void StartComing()
        {
            _masiAnimation.StartComing();
        }

        private void StartGoing()
        {
            _masiAnimation.StartGoing();
        }
        public void StartAttack()
        {
            _masiAttack.StartShooting();
            _masiAnimation.StartFire();
        }
        public void StopAttack()
        {
            _masiAttack.StopShooting();
            _masiAnimation.StopFire();
        }
    }
}
