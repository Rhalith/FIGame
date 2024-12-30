using System.Collections.Generic;
using Scripts.EventBus;
using Scripts.Events;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _musicClips;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private TMP_Text _radioText;
        [SerializeField] private int _visibleCharacters = 10; // Number of characters to display
        [SerializeField] private float _updateInterval = 0.2f; // Time interval for text updates

        private string _currentMusicName;
        private string _displayedText;
        private int _scrollIndex;
        private bool _canChange;
        private bool _shouldPlay;


        private void OnEnable()
        {
            EventBus<PlaySongEvent>.AddListener(PlaySong);
            EventBus<PlayerDeathEvent>.AddListener(StopSong);
        }

        private void OnDisable()
        {
            EventBus<PlaySongEvent>.RemoveListener(PlaySong);
            EventBus<PlayerDeathEvent>.RemoveListener(StopSong);
        }

        private void PlaySong(object sender, PlaySongEvent @event)
        {
            if(_audioSource.isPlaying) return;
            PlayRandomMusic();
            _radioText.enabled = true;
            InitializeDisplayedText();
            _canChange = true;
            _shouldPlay = true;
            InvokeRepeating(nameof(UpdateDisplayedText), _updateInterval, _updateInterval);
        }

        private void StopSong(object sender, PlayerDeathEvent @event)
        {
            _canChange = false;
            _shouldPlay = false;
            _audioSource.Stop();
            _radioText.enabled = false;
            CancelInvoke(nameof(UpdateDisplayedText));
        }

        private void Update()
        {
            // Check if the current song has finished playing
            if (!_audioSource.isPlaying && _audioSource.clip != null && _shouldPlay && !GameManager.Instance.IsPaused)
            {
                PlayRandomMusic();
                InitializeDisplayedText();
            }
        }

        private void PlayRandomMusic()
        {
            int randomIndex = Random.Range(0, _musicClips.Count);
            _audioSource.clip = _musicClips[randomIndex];
            _currentMusicName = _musicClips[randomIndex].name.ToUpper(); // Convert to uppercase for a radio feel
            _audioSource.Play();
        }

        private void InitializeDisplayedText()
        {
            // Extract the formatted song name
            _currentMusicName = FormatSongName(_currentMusicName);

            // Padding spaces to simulate a looping effect
            _displayedText = $"{_currentMusicName}    {_currentMusicName}    ";
            _scrollIndex = 0; // Reset scrolling index
        }

        private string FormatSongName(string fullName)
        {
            // Extract the part before the '｜' or '['
            string[] separators = { "｜", "[" };
            string[] parts = fullName.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            // Combine the main parts if available
            if (parts.Length > 1)
            {
                return $"{parts[0].Trim()} - {parts[1].Trim()}";
            }

            // Fallback to just the first part if no separators are found
            return parts[0].Trim();
        }


        private void UpdateDisplayedText()
        {
            if (_displayedText.Length == 0 || _visibleCharacters <= 0)
                return;

            // Extract the current segment of text to display
            string visibleText = GetVisibleText();
            _radioText.text = visibleText;

            // Increment the scroll index with wrapping
            _scrollIndex = (_scrollIndex + 1) % _displayedText.Length;
        }

        private string GetVisibleText()
        {
            int endIndex = (_scrollIndex + _visibleCharacters) % _displayedText.Length;
            if (endIndex > _scrollIndex)
            {
                // Simple substring if the range doesn't wrap
                return _displayedText.Substring(_scrollIndex, _visibleCharacters);
            }
                // Wrap around to the beginning of the text
            string part1 = _displayedText.Substring(_scrollIndex);
            string part2 = _displayedText.Substring(0, endIndex);
            return part1 + part2;
        }

        public void OnChangeSong()
        {
            if(!_canChange) return;
            int currentSongIndex = _musicClips.IndexOf(_audioSource.clip);
            int nextSongIndex = (currentSongIndex + 1) % _musicClips.Count;

            _audioSource.clip = _musicClips[nextSongIndex];
            _currentMusicName = _musicClips[nextSongIndex].name.ToUpper();
            _audioSource.Play();

            InitializeDisplayedText();
        }
    }
}