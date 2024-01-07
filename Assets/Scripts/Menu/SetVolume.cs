using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Menu
{
    public class SetVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        public void AdjustVolume(float volume)
        {
            _audioMixer.SetFloat("mainvolume", volume);
        }
    }
}