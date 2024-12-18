using System.Collections;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Menu
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioFinish : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private float _duration;

        public void FinishAudio()
        {
            _duration = _audioSource.clip.length;
            StartCoroutine(WaitForSound());
        }

        private IEnumerator WaitForSound()
        {
            EventBus<ResetCarPositionEvent>.Emit(this, new ResetCarPositionEvent());
            yield return new WaitForSeconds(_duration);
            EventBus<StartPitEnterEvent>.Emit(this, new StartPitEnterEvent());
            gameObject.SetActive(false);
        }
    }
}