using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ScrollBackground _scrollBg;
    [SerializeField] private GameObject _tireMenu;

    private PlayerManager _playerManager;
    private MasiManager _masiManager;
    
    public MasiManager MasiManager { set => _masiManager = value; }
    public PlayerManager PlayerManager { set => _playerManager = value; }


    public void EnterPitLane()
    {
        _animator.enabled = true;
        _scrollBg.Speed = 0.2f;
    }

    public void StartPitStop()
    {
        _scrollBg.Speed = 0;
        _tireMenu.SetActive(true);
    }

    public void AfterPitStop()
    {
        _scrollBg.Speed = 0.2f;
        _animator.SetBool("isTireSelected", true);
        _tireMenu.SetActive(false);
    }

    public void ExitPitLane()
    {
        _scrollBg.Speed = 0.4f;
        _animator.SetBool("isTireSelected", false);
        _masiManager.StartComing();
        _animator.enabled = false;
    }
}
