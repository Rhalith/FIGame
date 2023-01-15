using Assets.Scripts.Menu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Image _tyreLogo;

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
    [SerializeField] private Image _healthbar;
    [SerializeField] private Sprite _maxHealthbarSprite;
    [SerializeField] private Sprite _lewisHealthbarSprite;
    [SerializeField] private Sprite _softTyreLogoSprite;
    [SerializeField] private Sprite _mediumTyreLogoSprite;
    [SerializeField] private Sprite _hardTyreLogoSprite;
    #endregion
    #region RadioBar
    [Header("RadioBar")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _verstappenRadio;
    [SerializeField] private AudioClip _hamiltonRadio;
    [SerializeField] private TMP_Text _driverText;
    [SerializeField] private AudioFinish _audioFinish;
    #endregion
    public void SetupGame(bool isMax)
    {
        if (isMax)
        {
            ChangeDriver(0);
            ChangeHealthBar(0);
            ChangeDriverRadio(0);
        }
        else
        {
            ChangeDriver(1);
            ChangeHealthBar(1);
            ChangeDriverRadio(1);
        }
    }
    public void ChangeTyre(string tyre)
    {
        switch (tyre)
        {
            case "soft":
                _tyre.sprite = _softTyreSprite;
                _tyreLogo.sprite = _softTyreLogoSprite;
                break;
            case "medium":
                _tyre.sprite = _mediumTyreSprite;
                _tyreLogo.sprite = _mediumTyreLogoSprite;
                break;
            case "hard":
                _tyre.sprite = _hardTyreSprite;
                _tyreLogo.sprite = _hardTyreLogoSprite;
                break;
        }
    }

    private void ChangeDriver(int i)
    {
        if(i == 0) _car.sprite = _maxCarSprite;
        else _car.sprite= _lewisCarSprite;
    }

    private void ChangeHealthBar(int i)
    {
        if (i == 0) _healthbar.sprite = _maxHealthbarSprite;
        else _healthbar.sprite = _lewisHealthbarSprite;
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
