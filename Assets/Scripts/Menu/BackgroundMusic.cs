using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Menu
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioMixer _audioMixer;
        public void PlayMusic()
        {
            _audioSource.Play();
            _audioMixer.SetFloat("mainvolume", -50f);
        }
    }
}