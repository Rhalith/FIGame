using System.Collections;
using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;

namespace Scripts.Masi
{
    public class MasiAttack : MonoBehaviour
    {
        [SerializeField] private PenaltyController _penaltyController;
        [SerializeField] private GameObject _target;
        [SerializeField] private float _fireRate;

        private bool _shooting;

        private void OnEnable()
        {
            EventBus<IncreaseDifficultyEvent>.AddListener(IncreaseFireRate);
        }
        
        private void OnDisable()
        {
            EventBus<IncreaseDifficultyEvent>.RemoveListener(IncreaseFireRate);
        }

        private void IncreaseFireRate(object sender, IncreaseDifficultyEvent @event)
        {
            _fireRate *= @event.IncreaseRate;
        }

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


        private IEnumerator Shooting()
        {
            while (_shooting)
            {
                yield return new WaitForSeconds(1 / _fireRate);
                if (!_shooting) yield break;
                _penaltyController.GetPooledObject().Shoot(_target.transform);
            }
        }
    }
}
