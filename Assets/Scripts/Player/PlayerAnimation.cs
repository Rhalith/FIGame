using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ScrollBackground _scrollBg;


    private float _speedInPitLane;
    public void EnterPitLane()
    {
        _scrollBg.Speed = 0.2f;
    }

    public void StartPitStop()
    {
        _scrollBg.Speed = 0;
    }

    public void AfterPitStop()
    {
        _scrollBg.Speed = 0.2f;
    }

    public void ExitPitLane()
    {
        _scrollBg.Speed = 0.4f;
    }
}
