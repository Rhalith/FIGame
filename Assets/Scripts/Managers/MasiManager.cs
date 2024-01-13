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
        }
        
        private void OnDisable()
        {
            EventBus<PitLaneEntranceEvent>.RemoveListener(StartComing);
            EventBus<PlayerDeathEvent>.RemoveListener(StopMasiAttack);
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

        public void StartComing()
        {
            _masiAnimation.StartComing();
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
