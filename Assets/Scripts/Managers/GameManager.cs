using Scripts.EventBus;
using Scripts.Events;
using Scripts.Menu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Settings

        [Header("Settings")] [SerializeField] private float _increaseRate;

        #endregion

        #region Car

        [Header("Car")] [SerializeField] private Image _car;
        [SerializeField] private Sprite _maxCarSprite;
        [SerializeField] private Sprite _lewisCarSprite;

        #endregion

        #region CarTyre

        [Header("CarTyre")] [SerializeField] private Image _tyre;
        [SerializeField] private Sprite _softTyreSprite;
        [SerializeField] private Sprite _mediumTyreSprite;
        [SerializeField] private Sprite _hardTyreSprite;

        #endregion

        #region HealthBar

        [Header("HealthBar")] [SerializeField] private Image _tyreLogo;
        [SerializeField] private Image _healthbarImage;
        [SerializeField] private Sprite _maxHealthbarSprite;
        [SerializeField] private Sprite _lewisHealthbarSprite;
        [SerializeField] private Sprite _softTyreLogoSprite;
        [SerializeField] private Sprite _mediumTyreLogoSprite;
        [SerializeField] private Sprite _hardTyreLogoSprite;
        [SerializeField] private Healthbar _healthbar;

        #endregion

        #region RadioBar

        [Header("RadioBar")] [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _verstappenRadio;
        [SerializeField] private AudioClip _hamiltonRadio;
        [SerializeField] private TMP_Text _driverText;
        [SerializeField] private AudioFinish _audioFinish;

        #endregion

        #region Managers

        [Header("Managers")] [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private MasiManager _masiManager;
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private ScoreManager _scoreManager;

        #endregion

        #region Restart Properties

        [Header("Restart Properties")]
        [SerializeField] private GameObject _deathScreen;
        [SerializeField] private GameObject _mainGame;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private ScrollBackground _scrollBackground;

        #endregion
        
        
        #region PauseMenu

        [Header("PauseMenu")]
        [SerializeField] private List<AudioSource> _audioSources;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private TMP_Text _currentScoreText;
        [SerializeField] private TMP_Text _maxScoreText;

        private bool _isPaused;
        private bool _canPause;

        #endregion

        #region Getters & Setters

        public bool IsGameFinished
        {
            get => _isGameFinished;
            set => _isGameFinished = value;
        }

        public bool IsPaused => _isPaused;

        public bool CanPause
        {
            get => _canPause;
            set => _canPause = value;
        }

        public static GameManager Instance { get; private set; }

        #endregion

        private bool _isSetup;
        private bool _isMax;
        private bool _isGameFinished;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void OnEnable()
        {
            if (!_isSetup)
            {
                SetupGame(false);
            }
        }

        public void SetupGame(bool isMax)
        {
            // Emit a default tire setup without affecting health
            ChangeTyreVisuals(Tire.Soft);

            if (isMax)
            {
                ChangeDriver(0);
                ChangeHealthBarImage(0);
                ChangeDriverRadio(0);
            }
            else
            {
                ChangeDriver(1);
                ChangeHealthBarImage(1);
                ChangeDriverRadio(1);
            }

            _isSetup = true;
        }

        public void ResetGame()
        {
            _audioFinish.gameObject.SetActive(true);
            _scrollBackground.Speed = 0.4f;
            EventBus<SetPlayerMovementEvent>.Emit(this, new SetPlayerMovementEvent { CanMove = false });
            EventBus<ResetSafetyCarEvent>.Emit(this, new ResetSafetyCarEvent());
            _mainGame.SetActive(false);
            _mainMenu.SetActive(true);
            _isGameFinished = false;
        }

        public void ContinueGame()
        {
            _scrollBackground.Speed = 0.4f;
            EventBus<IncreaseDifficultyEvent>.Emit(this, new IncreaseDifficultyEvent { IncreaseRate = _increaseRate });
            _canPause = true;
            StartCoroutine(ContinueGameCoroutine());
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void PauseGame()
        {
            if(!_canPause) return;
            _currentScoreText.text = $"Current Score: {_scoreManager._score}";
            _maxScoreText.text = $"Max Score: {PlayerPrefs.GetFloat("MaxScore", 0f)}";
            _isPaused = !_isPaused;
            EventBus<CheckSelectableElementEvent>.Emit(this, new CheckSelectableElementEvent { CanSelect = _isPaused });
            Time.timeScale = _isPaused ? 0 : 1;
            _pauseMenu.SetActive(_isPaused);
            foreach (var audioSource in _audioSources)
            {
                if(!audioSource.gameObject.activeInHierarchy) continue;
                if (_isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.Play();
                }
            }
        }

        public void SetTyre(string tire)
        {
            switch (tire)
            {
                case "soft":
                    ChangeTyre(Tire.Soft);
                    break;
                case "medium":
                    ChangeTyre(Tire.Medium);
                    break;
                case "hard":
                    ChangeTyre(Tire.Hard);
                    break;
            }
        }

        private void ChangeTyre(Tire tire)
        {
            ChangeTyreVisuals(tire);
            _canPause = true;
            EventBus<ChangeTireEvent>.Emit(this, new ChangeTireEvent { ChosenTire = tire });
        }

        private void ChangeTyreVisuals(Tire tire)
        {
            switch (tire)
            {
                case Tire.Soft:
                    _tyre.sprite = _softTyreSprite;
                    _tyreLogo.sprite = _softTyreLogoSprite;
                    break;
                case Tire.Medium:
                    _tyre.sprite = _mediumTyreSprite;
                    _tyreLogo.sprite = _mediumTyreLogoSprite;
                    break;
                case Tire.Hard:
                    _tyre.sprite = _hardTyreSprite;
                    _tyreLogo.sprite = _hardTyreLogoSprite;
                    break;
            }
        }

        private void ChangeDriver(int i)
        {
            if (i == 0) _car.sprite = _maxCarSprite;
            else _car.sprite = _lewisCarSprite;
        }

        private void ChangeHealthBarImage(int i)
        {
            if (i == 0) _healthbarImage.sprite = _maxHealthbarSprite;
            else _healthbarImage.sprite = _lewisHealthbarSprite;
        }

        private void ChangeDriverRadio(int i)
        {
            SetDriverSettings(i);
            _audioSource.Play();
            _audioFinish.FinishAudio();
        }

        private void SetDriverSettings(int i)
        {
            if (i == 0)
            {
                _audioSource.clip = _verstappenRadio;
                _driverText.text = "VERSTAPPEN";
                var color = new Color(0.282353f, 0.4431373f, 0.7176471f);
                _driverText.color = color;
                _timerManager.ChangeTimerColor(color);
            }
            else
            {
                _audioSource.clip = _hamiltonRadio;
                _driverText.text = "HAMILTON";
                var color = new Color(0.4705883f, 0.8039216f, 0.7450981f);
                _driverText.color = color;
                _timerManager.ChangeTimerColor(color);
            }
        }

        private IEnumerator ContinueGameCoroutine()
        {
            EventBus<ResetCarPositionEvent>.Emit(this, new ResetCarPositionEvent());
            yield return new WaitForSeconds(1f);
            EventBus<StartPitEnterEvent>.Emit(this, new StartPitEnterEvent());
        }


        public Dictionary<string, float> GetScore()
        {
            return new()
            {
                { "MaxScore", PlayerPrefs.GetFloat("MaxScore", 0f) },
                { "Score", _scoreManager._score }
            };
        }
    }

    public enum Tire
    {
        Soft,
        Medium,
        Hard
    }
}