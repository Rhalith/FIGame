using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasiManager : MonoBehaviour
{
    [SerializeField] private MasiAttack _masiAttack;
    [SerializeField] private MasiAnimation _masiAnimation;

    public void StartComing()
    {
        _masiAnimation.StartComing();
    }
    public void StartAttack()
    {
        _masiAttack.StartShooting();
        _masiAnimation.StartFire();
    }
    public void StopAttack()
    {
        _masiAttack.StopShooting();
        _masiAnimation.StopFire();
    }
}
