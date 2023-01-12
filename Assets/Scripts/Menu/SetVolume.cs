using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Menu
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