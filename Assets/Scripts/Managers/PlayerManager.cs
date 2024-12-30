using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using Scripts.Player;
using TMPro;
using UnityEngine;

namespace Scripts.Managers
{
   public class PlayerManager : MonoBehaviour
   {
      #region Player Scripts

      [Header("Player Scripts")]
      [SerializeField] private PlayerMovement _playerMovement;

      #endregion

      #region Other Components

      [Header("Other Components")]
      [SerializeField]
      private GameManager _gameManager;

      [SerializeField] private ScrollBackground _scrollBackground;
      [SerializeField] private GameObject _tireMenu;
      [SerializeField] private Animator _animator;
      [SerializeField] private Rigidbody2D _rigidbody;
      [SerializeField] private GameObject _deathScreen;
      [SerializeField] private TMP_Text _scoreText;

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
         var scoreDict = _gameManager.GetScore();

         float maxScore = scoreDict["MaxScore"];
         float score = scoreDict["Score"];

         DOTween.To(() => _scrollBackground.Speed, x => _scrollBackground.Speed = x, 0, 1f)
             .OnComplete(() =>
             {
                _deathScreen.SetActive(true);
                _scoreText.text = $"New Score: {score:F0}\n\nMax Score: {maxScore:F0}";
             });
         EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = true });
         EventBus<TyreAnimationEvent>.Emit(this, new TyreAnimationEvent { ShouldStop = true });
      }

      private void PitStop(object sender, PitStopEvent @event)
      {
         if (@event.IsStart)
         {
            ChangeBackGroundSpeed(0, 1f);
            _tireMenu.SetActive(true);
            GameManager.Instance.CanPause = false;
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
            EventBus<PlaySongEvent>.Emit(this, new PlaySongEvent());
            EventBus<StartTimerEvent>.Emit(this, new StartTimerEvent());
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = true});
         }
      }


      private void ChangeBackGroundSpeed(float speed, float duration)
      {
         DOTween.To(() => _scrollBackground.Speed, x => _scrollBackground.Speed = x, speed, duration);
      }
   }
}