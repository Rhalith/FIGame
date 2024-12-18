using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Player Scripts

        [Header("Player Scripts")] [SerializeField]
        private PlayerAnimation _playerAnimation;

        [SerializeField] private PlayerMovement _playerMovement;

        #endregion

        #region Other Components

        [Header("Other Components")] [SerializeField]
        private GameManager _gameManager;

        [SerializeField] private ScrollBackground _scrollBackground;
        [SerializeField] private GameObject _tireMenu;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GameObject _deathScreen;

        #endregion

        private void OnEnable()
        {
            EventBus<PitLaneEntranceEvent>.AddListener(PitLaneEntrance);
            EventBus<PitStopEvent>.AddListener(PitStop);
            EventBus<PlayerDeathEvent>.AddListener(KillPlayer);
        }

        private void OnDisable()
        {
            EventBus<PitLaneEntranceEvent>.RemoveListener(PitLaneEntrance);
            EventBus<PitStopEvent>.RemoveListener(PitStop);
            EventBus<PlayerDeathEvent>.RemoveListener(KillPlayer);
        }

        private void KillPlayer(object sender, PlayerDeathEvent @event)
        {
            DOTween.To(() => _scrollBackground.Speed, x => _scrollBackground.Speed = x, 0, 1f)
                .OnComplete(() => _deathScreen.SetActive(true));
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = true });
            EventBus<TyreAnimationEvent>.Emit(this, new TyreAnimationEvent { ShouldStop = true });
        }

        private void PitStop(object sender, PitStopEvent @event)
        {
            if (@event.IsStart)
            {
                ChangeBackGroundSpeed(0, 1f);
                _tireMenu.SetActive(true);
                EventBus<TyreAnimationEvent>.Emit(this, new TyreAnimationEvent { ShouldStop = true });
                EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = true });
            }
            else
            {
                ChangeBackGroundSpeed(0.2f, 1f);
                _animator.SetBool("isTireSelected", true);
                _tireMenu.SetActive(false);
                EventBus<TyreAnimationEvent>.Emit(this, new TyreAnimationEvent { ShouldStop = false });
                EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
            }
        }

        private void PitLaneEntrance(object sender, PitLaneEntranceEvent @event)
        {
            if (@event.IsEntering)
            {
                ChangeBackGroundSpeed(0.2f, 1f);
            }
            else
            {
                ChangeBackGroundSpeed(0.4f, 1f);
                _animator.SetBool("isTireSelected", false);
                _animator.enabled = false;
            }
        }


        private void ChangeBackGroundSpeed(float speed, float duration)
        {
            DOTween.To(() => _scrollBackground.Speed, x => _scrollBackground.Speed = x, speed, duration);
        }
    }
}