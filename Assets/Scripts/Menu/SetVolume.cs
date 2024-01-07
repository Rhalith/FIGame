using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Menu
{
    public class SetVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        private void Awake()
        {
            _audioMixer.SetFloat("mainvolume", -50f);
        }

        public void AdjustVolume(float volume)
        {
            _audioMixer.SetFloat("mainvolume", volume);
        }
    }
}