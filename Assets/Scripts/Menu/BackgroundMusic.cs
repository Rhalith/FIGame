using UnityEngine;

namespace Menu
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        public void PlayMusic()
        {
            _audioSource.Play();
        }
    }
}