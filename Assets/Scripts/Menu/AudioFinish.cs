using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Menu
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioFinish : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private float _duration;

        [SerializeField] private UnityEvent onFinishSound;

        public void FinishAudio()
        {
            _duration = _audioSource.clip.length;
            StartCoroutine(WaitForSound());
        }

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(_duration);
            onFinishSound.Invoke();
        }
    }
}