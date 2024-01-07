using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Masi
{
    public class PenaltyController : MonoBehaviour
    {
        [SerializeField] private GameObject _masi;
        [SerializeField] private GameObject _penaltyPrefab;

        [SerializeField] List<PenaltyHit> _penaltyHits;
        [SerializeField] List<Sprite> _fiveSecondSprites;
        [SerializeField] List<Sprite> _tenSecondSprites;

        private void PreparePenalty(PenaltyHit penalty)
        {
            int i = Random.Range(0, 2);
            int j = Random.Range(0, 2);
            if (i == 0)
            {
                penalty.ChangePenalty(Penalty.FiveSeconds, _fiveSecondSprites[j]);
            }
            else
            {
                penalty.ChangePenalty(Penalty.TenSeconds, _tenSecondSprites[j]);
            }
        }
        public PenaltyHit GetPooledObject()
        {
            for (int i = 0; i < _penaltyHits.Count; i++)
            {
                if (!((PenaltyHit)_penaltyHits[i]).gameObject.activeInHierarchy)
                {
                    PreparePenalty(_penaltyHits[i]);
                    return (PenaltyHit)_penaltyHits[i];
                }
            }

            GameObject obj = (GameObject)Instantiate(_penaltyPrefab);
            PenaltyHit objPenaltyHit = obj.GetComponent<PenaltyHit>();
            _penaltyHits.Add(objPenaltyHit);
            return objPenaltyHit;
        }
    }
}
