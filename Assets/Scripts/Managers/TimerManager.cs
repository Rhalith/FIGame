using System.Collections;
using Scripts.EventBus;
using Scripts.Events;
using TMPro;
using UnityEngine;

namespace Scripts.Managers
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private int _time; // Total time in seconds
        [SerializeField] private TMP_Text _timerText;

        private float _remainingTime; // Time remaining in seconds
        private Coroutine _countdownCoroutine; // Reference to the running coroutine
        private bool _isPaused; // To track if the timer is paused

        private void OnEnable()
        {
            EventBus<StartTimerEvent>.AddListener(StartTimer);
            EventBus<PlayerDeathEvent>.AddListener(StopTimer);
            EventBus<CallSafetyCarEvent>.AddListener(PauseTimer);
            EventBus<SendSafetyCarEvent>.AddListener(ResumeTimer);
            EventBus<ResetSafetyCarEvent>.AddListener(ResetSafetyCarTimer);
        }

        private void OnDisable()
        {
            EventBus<StartTimerEvent>.RemoveListener(StartTimer);
            EventBus<PlayerDeathEvent>.RemoveListener(StopTimer);
            EventBus<CallSafetyCarEvent>.RemoveListener(PauseTimer);
            EventBus<SendSafetyCarEvent>.RemoveListener(ResumeTimer);
            EventBus<ResetSafetyCarEvent>.RemoveListener(ResetSafetyCarTimer);
        }

        private void PauseTimer(object sender, CallSafetyCarEvent @event)
        {
            if (!_isPaused && _countdownCoroutine != null)
            {
                _timerText.enabled = false;
                _isPaused = true;
                StopCoroutine(_countdownCoroutine); // Stop the coroutine to pause the timer
                _countdownCoroutine = null;
            }
        }

        private void ResumeTimer(object sender, SendSafetyCarEvent @event)
        {
            if (_isPaused)
            {
                _timerText.enabled = true;
                _isPaused = false;
                _countdownCoroutine = StartCoroutine(CountdownTimer()); // Restart the timer coroutine
            }
        }

        private void StartTimer(object sender, StartTimerEvent @event)
        {
            _timerText.enabled = true;
            _remainingTime = _time;
            UpdateTimerText();

            // Start the countdown and store the coroutine reference
            _countdownCoroutine = StartCoroutine(CountdownTimer());
        }

        private void StopTimer(object sender, PlayerDeathEvent @event)
        {
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null; // Clear the reference
            }
        }
        
        private void ResetSafetyCarTimer(object sender, ResetSafetyCarEvent @event)
        {
            _isPaused = false;
        }

        private IEnumerator CountdownTimer()
        {
            while (_remainingTime > 0)
            {
                if (!_isPaused && !GameManager.Instance.IsGameFinished)
                {
                    yield return null; // Wait for the next frame
                    _remainingTime -= Time.deltaTime;
                    UpdateTimerText();
                }
                else
                {
                    yield return null; // Allow coroutine to continue without decrementing time
                }
            }

            _remainingTime = 0;
            UpdateTimerText();
            OnTimerEnd();
        }

        private void UpdateTimerText()
        {
            if (_remainingTime <= 10)
            {
                _timerText.text = $"{_remainingTime:F2} s";
            }
            else
            {
                int seconds = Mathf.CeilToInt(_remainingTime);
                _timerText.text = $"{seconds} s";
            }
        }

        private void ResetTimer()
        {
            _remainingTime = _time;
            UpdateTimerText();
        }
        
        public void ChangeTimerColor(Color color)
        {
            _timerText.color = color;
            ResetTimer();
        }

        private void OnTimerEnd()
        {
            EventBus<TimeEndEvent>.Emit(this, new TimeEndEvent());
            EventBus<DisablePenaltiesEvent>.Emit(this, new DisablePenaltiesEvent());
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent{CanMove = false});
        }
    }
}
