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

        private void OnEnable()
        {
            EventBus<StartTimerEvent>.AddListener(StartTimer);
            EventBus<PlayerDeathEvent>.AddListener(StopTimer);
        }

        private void OnDisable()
        {
            EventBus<StartTimerEvent>.RemoveListener(StartTimer);
            EventBus<PlayerDeathEvent>.RemoveListener(StopTimer);
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

        private IEnumerator CountdownTimer()
        {
            while (_remainingTime > 0)
            {
                yield return null; // Wait for the next frame
                _remainingTime -= Time.deltaTime;
                UpdateTimerText();
            }

            _remainingTime = 0;
            UpdateTimerText();
            OnTimerEnd();
        }

        private void UpdateTimerText()
        {
            if (_remainingTime <= 10)
            {
                // Display seconds and split seconds
                _timerText.text = $"{_remainingTime:F2} s";
            }
            else
            {
                // Display only seconds
                int seconds = Mathf.CeilToInt(_remainingTime);
                _timerText.text = $"{seconds} s";
            }
        }

        private void OnTimerEnd()
        {
            EventBus<TimeEndEvent>.Emit(this, new TimeEndEvent());
        }
    }
}
