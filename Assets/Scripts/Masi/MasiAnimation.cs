using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Masi
{
    public class MasiAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _moveAnimator;
        [SerializeField] private Animator _fireAnimator;


        private void OnEnable()
        {
            EventBus<IncreaseDifficultyEvent>.AddListener(IncreaseAnimationSpeed);
        }
        
        private void OnDisable()
        {
            EventBus<IncreaseDifficultyEvent>.RemoveListener(IncreaseAnimationSpeed);
        }

        private void IncreaseAnimationSpeed(object sender, IncreaseDifficultyEvent @event)
        {
            _moveAnimator.speed *= @event.IncreaseRate;
            _fireAnimator.speed *= @event.IncreaseRate;
        }

        public void StartComing()
        {
            SetGoing(0);
            SetComing(1);
        }
        public void StartGoing()
        {
            SetComing(0);
            SetGoing(1);
        }
        public void StartFire()
        {
            SetFire(1);
        }

        public void StopFire()
        {
            SetFire(0);
        }

        private void SetFire(int i)
        {
            _fireAnimator.SetBool("isReady", i != 0);
        }
        private void SetGoing(int i)
        {
            _moveAnimator.SetBool("going", i != 0);
        }
        private void SetComing(int i)
        {
            _moveAnimator.SetBool("coming", i != 0);
        }

    }
}
