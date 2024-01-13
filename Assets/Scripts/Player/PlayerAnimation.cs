using System;
using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
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