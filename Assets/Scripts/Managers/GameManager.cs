using Scripts.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {



        #region Car
        [Header("Car")]
        [SerializeField] private Image _car;
        [SerializeField] private Sprite _maxCarSprite;
        [SerializeField] private Sprite _lewisCarSprite;
        #endregion
        #region CarTyre
        [Header("CarTyre")]
        [SerializeField] private Image _tyre;
        [SerializeField] private Sprite _softTyreSprite;
        [SerializeField] private Sprite _mediumTyreSprite;
        [SerializeField] private Sprite _hardTyreSprite;
        #endregion
        #region HealthBar
        [Header("HealthBar")]
        [SerializeField] private Image _tyreLogo;
        [SerializeField] private Image _healthbarImage;
        [SerializeField] private Sprite _maxHealthbarSprite;
        [SerializeField] private Sprite _lewisHealthbarSprite;
        [SerializeField] private Sprite _softTyreLogoSprite;
        [SerializeField] private Sprite _mediumTyreLogoSprite;
        [SerializeField] private Sprite _hardTyreLogoSprite;
        [SerializeField] private Healthbar _healthbar;
        #endregion
        #region RadioBar
        [Header("RadioBar")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _verstappenRadio;
        [SerializeField] private AudioClip _hamiltonRadio;
        [SerializeField] private TMP_Text _driverText;
        [SerializeField] private AudioFinish _audioFinish;
        #endregion
        #region Managers
        [Header("Managers")]
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private MasiManager _masiManager;
        #endregion
        #region Restart Properties
        [SerializeField] private GameObject _deathScreen;
        [SerializeField] private GameObject _mainGame;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private ScrollBackground _scrollBackground;
        #endregion
        #region Getters & Setters
        public Healthbar Healthbar { get => _healthbar; }
        public PlayerManager PlayerManager { get => _playerManager; }
        public MasiManager MasiManager { get => _masiManager; }
        #endregion
        public void SetupGame(bool isMax)
        {
            _playerManager.SetMasiManager(_masiManager);
            _playerManager.SetPlayerManager();
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
        }
        public void ResetGame()
        {
            _audioFinish.gameObject.SetActive(true);
            _scrollBackground.Speed = 0.4f;
            _mainGame.SetActive(false);
            _mainMenu.SetActive(true);
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
            _playerManager.PlayerSpecs.ChangePlayerSpeed(tire);
            _playerManager.PlayerSpecs.ResetPlayerHealth();
            _healthbar.SetHealth(_playerManager.PlayerSpecs.PlayerHealth);
        }

        private void ChangeDriver(int i)
        {
            if(i == 0) _car.sprite = _maxCarSprite;
            else _car.sprite= _lewisCarSprite;
        }

        private void ChangeHealthBarImage(int i)
        {
            if (i == 0) _healthbarImage.sprite = _maxHealthbarSprite;
            else _healthbarImage.sprite = _lewisHealthbarSprite;
        }

        private void ChangeDriverRadio(int i)
        {
            if (i == 0)
            {
                _audioSource.clip = _verstappenRadio;
                _driverText.text = "VERSTAPPEN";
                _driverText.color = new Color(0.282353f, 0.4431373f, 0.7176471f);
            }
            else
            {
                _audioSource.clip = _hamiltonRadio;
                _driverText.text = "HAMILTON";
                _driverText.color = new Color(0.4705883f, 0.8039216f, 0.7450981f);
            }
            _audioSource.Play();
            _audioFinish.FinishAudio();
        }
    }

    public enum Tire
    {
        Soft,
        Medium,
        Hard
    }
}