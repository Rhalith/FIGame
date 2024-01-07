using System.Collections;
using UnityEngine;

namespace Scripts.Masi
{
    public class MasiAttack : MonoBehaviour
    {
        [SerializeField] private PenaltyController _penaltyController;
        [SerializeField] private GameObject _target;
        [SerializeField] private float _fireRate;

        private bool _shooting;

        public void StartShooting()
        {
            _shooting = true;
            StartCoroutine(Shooting());
        }

        public void StopShooting()
        {
            _shooting = false;
            StopCoroutine(Shooting());
        }


        IEnumerator Shooting()
        {
            while (_shooting)
            {
                yield return new WaitForSeconds(1 / _fireRate);
                _penaltyController.GetPooledObject().Shoot(_target.transform);
            }
        }
    }
}
