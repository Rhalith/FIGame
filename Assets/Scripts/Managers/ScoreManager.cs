using DG.Tweening;
using Scripts.EventBus;
using Scripts.Events;
using TMPro;
using UnityEngine;

namespace Scripts.Managers
{
   public class ScoreManager : MonoBehaviour
   {
      [SerializeField] private TMP_Text _scoreText;
      [SerializeField] private GameObject _scorePanel;
      [SerializeField] private Transform _scoreTextTransform;
      [SerializeField] private float _animationDuration = 1.0f;
      [SerializeField] private int _scoreMultiplier = 10;

      public float _score { get; private set; }
      public float _maxScore { get; private set; }
      private Vector3 _originalPosition;

      private const string MaxScoreKey = "MaxScore";

      private void Awake()
      {
         // Save the original position of the text
         _originalPosition = _scoreTextTransform.localPosition;

         // Load the maximum score from PlayerPrefs
         _maxScore = PlayerPrefs.GetFloat(MaxScoreKey, 0f);
      }

      private void OnEnable()
      {
         EventBus<UpdateScoreEvent>.AddListener(UpdateScore);
      }

      private void OnDisable()
      {
         EventBus<UpdateScoreEvent>.RemoveListener(UpdateScore);
      }

      private void UpdateScore(object sender, UpdateScoreEvent @event)
      {
         _scoreText.text = @event.ScoreChange.ToString();
         _scoreText.enabled = true;
         // Animate the score text
         AnimateScoreText(@event.ScoreChange);
      }

      private void AnimateScoreText(float scoreChange)
      {
         float targetScoreChange = scoreChange * _scoreMultiplier;

         // Sequence for animations
         Sequence sequence = DOTween.Sequence();

         // Move the score text to the center of the screen
         sequence.Append(_scoreTextTransform.DOLocalMove(new Vector3(0, 0, 0), _animationDuration).SetEase(Ease.InOutQuad));

         // Smoothly multiply the score change
         sequence.Append(DOTween.To(() => scoreChange, x =>
         {
            _scoreText.text = x.ToString("F0");
         }, targetScoreChange, _animationDuration).SetEase(Ease.InOutQuad));

         // Scale the score text at the end of multiplication for a finishing effect
         sequence.Append(_scoreTextTransform.DOScale(3f, 0.3f).SetEase(Ease.OutQuad)); // Scale up
         sequence.Append(_scoreTextTransform.DOScale(2f, 0.3f).SetEase(Ease.OutQuad)); // Scale back to normal

         // Display the final text
         sequence.AppendCallback(() =>
         {
            _score += targetScoreChange; // Add the multiplied score change to the total score,
            CheckAndUpdateMaxScore();
            _scoreText.text = $"Your Health = {scoreChange}\nYour New Score: {_score:F0}\n\nMax Score: {_maxScore:F0}";
         });

         // Wait a little and reset the text position
         sequence.AppendInterval(0.5f);

         // Show the score panel after animation
         sequence.AppendCallback(() =>
         {
            GameManager.Instance.CanPause = false;
            _scorePanel.SetActive(true);
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = true });
         });
      }

      private void CheckAndUpdateMaxScore()
      {
         if (_score > _maxScore)
         {
            _maxScore = _score;
            PlayerPrefs.SetFloat(MaxScoreKey, _maxScore); // Save the new max score
            PlayerPrefs.Save();
         }
      }

      public void ResetTextPosition()
      {
         EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = false });
         _scoreTextTransform.localPosition = _originalPosition;
      }
   }
}
