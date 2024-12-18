using Scripts.Events;
using Scripts.EventBus;
using UnityEngine;

namespace Scripts.Player
{
    public class TyreAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;


        private void OnEnable()
        {
            EventBus<TyreAnimationEvent>.AddListener(StartTyreAnimation);
        }
        
        private void OnDisable()
        {
            EventBus<TyreAnimationEvent>.RemoveListener(StartTyreAnimation);
        }

        private void StartTyreAnimation(object sender, TyreAnimationEvent @event)
        {
            _animator.SetBool("inPit", @event.ShouldStop);
        }
    }
}