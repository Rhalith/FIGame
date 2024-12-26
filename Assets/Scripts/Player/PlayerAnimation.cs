using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void OnEnable()
        {
            EventBus<StartPitEnterEvent>.AddListener(StartPitEnter);
        }
        
        private void OnDisable()
        {
            EventBus<StartPitEnterEvent>.RemoveListener(StartPitEnter);
        }

        private void StartPitEnter(object sender, StartPitEnterEvent @event)
        {
            animator.enabled = true;
            animator.SetTrigger("startPit");
        }

        public void EnterPitLane()
        {
            EventBus<PitLaneEntranceEvent>.Emit(this, new PitLaneEntranceEvent { IsEntering = true });
        }

        public void ExitPitLane()
        {
            EventBus<PitLaneEntranceEvent>.Emit(this, new PitLaneEntranceEvent { IsEntering = false });
        }

        public void StartPitStop()
        {
            EventBus<PitStopEvent>.Emit(this, new PitStopEvent { IsStart = true });
        }

        public void AfterPitStop()
        {
            EventBus<PitStopEvent>.Emit(this, new PitStopEvent { IsStart = false });
        }
    }
}