using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasiAnimation : MonoBehaviour
{
    [SerializeField] private Animator _moveAnimator;
    [SerializeField] private Animator _fireAnimator;

    [SerializeField] private MasiAttack _masiAttack;

    public void StartComing()
    {
        SetGoing(0);
        SetComing(1);
    }
    public void StartGoing()
    {
        SetComing(0);
        SetGoing(1);
    }
    public void StartFire()
    {
        SetFire(1);
        _masiAttack.StartShooting();
    }

    public void StopFire()
    {
        SetFire(0);
        _masiAttack.StopShooting();
    }

    private void SetFire(int i)
    {
        _fireAnimator.SetBool("isReady", i != 0);
    }
    private void SetGoing(int i)
    {
        _moveAnimator.SetBool("going", i != 0);
    }
    private void SetComing(int i)
    {
        _moveAnimator.SetBool("coming", i != 0);
    }

}
