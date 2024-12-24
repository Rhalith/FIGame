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
      [SerializeField] List<Sprite> powerUpSprites;

      private void PreparePenalty(PenaltyHit penalty)
      {
         int i = Random.Range(0, 5);
         int j = Random.Range(0, 2);
         

         switch (i)
         {
            case 1:
               penalty.ChangePenalty(Penalty.TenSeconds, _tenSecondSprites[j]);
               break;
            default:
               penalty.ChangePenalty(Penalty.FiveSeconds, _fiveSecondSprites[j]);
               break;
         }
         
      }
      public PenaltyHit GetPooledObject()
      {
         for (int i = 0; i < _penaltyHits.Count; i++)
         {
            if (!_penaltyHits[i].gameObject.activeInHierarchy)
            {
               PreparePenalty(_penaltyHits[i]);
               return _penaltyHits[i];
            }
         }

         GameObject obj = Instantiate(_penaltyPrefab);
         PenaltyHit objPenaltyHit = obj.GetComponent<PenaltyHit>();
         _penaltyHits.Add(objPenaltyHit);
         return objPenaltyHit;
      }
   }
}
